using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Azure;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Middleware.Order;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Middleware.Esim;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static Raven.Client.Linq.LinqPathProvider;

namespace eSim.Implementations.Services.Esim
{
    public class EsimService : IEsimService
    {
        private readonly IConsumeApi _consumeApi;
        private readonly ApplicationDbContext _db;
        public EsimService(IConsumeApi consumeApi, ApplicationDbContext db)
        {
            _consumeApi = consumeApi;
            _db = db;
        }

        #region Apply bundle to a new esim
        public async Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToEsimAsync(ApplyBundleToEsimRequest input, string loggedUser)
        {
            var result = new Result<ApplyBundleToEsimResponse>();
            ApplyBundleToEsimResponse? response = new();

            var url = $"{BusinessManager.BaseURL}/esims/apply";

            try
            {
                response = await _consumeApi.PostApi<ApplyBundleToEsimResponse, ApplyBundleToEsimRequest>(url, input);

                if (response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;
                    return result;
                }

                if (response.Message is not null)
                {
                    result.Success = false;
                    result.Message = response.Message;

                    result.StatusCode = result.Message == BusinessManager.InternalServerError ? StatusCodes.Status500InternalServerError : StatusCodes.Status404NotFound;

                    return result;
                }

                result.Data = response;
                result.StatusCode = StatusCodes.Status200OK;

                #region Update Subscriber Inventory

                var subscriberInventoryUpdate = await UpdateInventory(loggedUser, input.Name);

                #endregion

                #region Esim Entry

                var fetchIccid = response.Esims.Select(u => u.Iccid).FirstOrDefault();

                string esimListUrl = $"{BusinessManager.BaseURL}/esims?filter={fetchIccid}&filterBy=iccid";

                var esimListResponse = await _consumeApi.GetApii<ListEsimsResponse>(esimListUrl);

                if (!esimListResponse.Success)
                {
                    //send email with full response and ask admin to manually dump data

                    result.Success = false;
                    result.Message = BusinessManager.DataDumpingIssue;

                    return result;
                }

                var esimDetails = esimListResponse.Data?.Esims?.Select(u => new Esims()
                {
                    Id = Guid.NewGuid().ToString(),
                    CustomerRef = u.CustomerRef ?? string.Empty,
                    LastAction = u.LastAction ?? string.Empty,
                    AssignedDate = u.ActionDate,
                    Iccid = u.Iccid ?? string.Empty,
                    ActionDate = u.ActionDate,
                    SubscriberId = loggedUser,
                    Physical = u.Physical,
                }).FirstOrDefault();

                if (esimDetails is not null)
                {
                    await _db.Esims.AddRangeAsync(esimDetails);
                    await _db.SaveChangesAsync();
                }

                #endregion

                #region Applied Esim Bundles Entry

                var esim = response.Esims.Select(u => new AppliedEsimBundles()
                {
                    Id = Guid.NewGuid().ToString(),
                    Iccid = u.Iccid,
                    AssignedDate = BusinessManager.GetDateTimeNow(),
                    BundleName = u.Bundle,
                }).FirstOrDefault();

                if (esim is not null)
                {
                    await _db.AppliedEsimBundles.AddAsync(esim);
                    await _db.SaveChangesAsync();
                }

                #endregion
            }
            catch (Exception ex)
            {
                //send email for manual data dumping

                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
        #endregion

        #region Apply bundle to an existing esim
        public async Task<Result<ApplyBundleToEsimResponse>> ApplyBundleToExistingEsimAsync(ApplyBundleToExistingEsimRequest input, string loggedUser)
        {
            var result = new Result<ApplyBundleToEsimResponse>();
            ApplyBundleToEsimResponse? response = new();

            var url = $"{BusinessManager.BaseURL}/esims/apply";
            try
            {
                var iccid = await _db.Esims.FirstOrDefaultAsync(u => u.Iccid == input.Iccid && u.SubscriberId == loggedUser);

                if (iccid is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.InvalidIccid;
                    result.StatusCode = StatusCodes.Status400BadRequest;

                    return result;
                }

                response = await _consumeApi.PostApi<ApplyBundleToEsimResponse, ApplyBundleToExistingEsimRequest>(url, input);

                if (response is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Exception;
                    return result;
                }

                if (response.Message is not null)
                {
                    result.Success = false;
                    result.Message = response.Message;

                    if (result.Message == BusinessManager.IncompatibleBundle)
                    {
                        result.StatusCode = StatusCodes.Status400BadRequest;
                    }
                    else if (result.Message == BusinessManager.NotFound)
                    {
                        result.StatusCode = StatusCodes.Status404NotFound;
                    }
                    else
                    {
                        result.StatusCode = StatusCodes.Status500InternalServerError;
                    }

                    return result;
                }

                result.Data = response;
                result.StatusCode = (int)HttpStatusCode.OK;

                #region Update Subscriber Inventory

                var subscriberInventoryUpdate = await UpdateInventory(loggedUser, input.Name);

                #endregion

                #region Applied Esim Bundles Entry

                var esim = response.Esims.Select(u => new AppliedEsimBundles()
                {
                    Id = Guid.NewGuid().ToString(),
                    Iccid = u.Iccid,
                    AssignedDate = BusinessManager.GetDateTimeNow(),
                    BundleName = u.Bundle,
                }).FirstOrDefault();

                if (esim is not null)
                {
                    await _db.AppliedEsimBundles.AddAsync(esim);


                    await _db.SaveChangesAsync();
                }

                #endregion

            }
            catch (Exception ex)
            {
                //send email for manual data dumping

                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #endregion

        #region  CheckeSIMandBundleCompatibility
        public async Task<Result<EsimCompatibilityResponseDTO>> CheckeSIMandBundleCompatibilityAsync(EsimCompatibilityRequestDto request)
        {
            var result = new Result<EsimCompatibilityResponseDTO>();
            string url = $" {BusinessManager.BaseURL}/esims/{request.Iccid}/compatible/{request.Bundle}";



            try
            {
                var response = await _consumeApi.GetApi<EsimCompatibilityResponseDTO>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotCompatible;
                    return result;
                }
                if (response.Message is not null)

                {
                    result.Success = false;
                    result.Message = response.Message;
                    return result;
                }
                result.Success = response.Compatible;
                result.Data = response;
                result.Message = response.Compatible ? BusinessManager.EsimCompatible : BusinessManager.EsimNotCompatible;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
        #endregion

        #region Download QR
        public async Task<Result<byte[]>> DownloadQRAsync(string iccid)
        {
            var result = new Result<byte[]>();

            string url = $"{BusinessManager.BaseURL}/esims/{iccid}/qr";

            try
            {
                result = await _consumeApi.GetApii<byte[]>(url);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
        #endregion

     

        #region Esim details
        public async Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid, string? additionalfields)
        {
            var result = new Result<GetEsimDetailsResponse>();
            
            string url = additionalfields is not null ? $"{BusinessManager.BaseURL}/esims/{iccid}?additionalFields={additionalfields}" : $"{BusinessManager.BaseURL}/esims/{iccid}";

            try
            {
                result = await _consumeApi.GetApii<GetEsimDetailsResponse>(url);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.StatusCode = StatusCodes.Status500InternalServerError;
                result.Message = ex.Message;
            }
            return result;
        }
        #endregion

        #region Esim history

        public async Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid)
        {
            var result = new Result<GetEsimHistoryResponse>();
            string url = $"{BusinessManager.BaseURL}/esims/{iccid}/history";

            try
            {
                result = await _consumeApi.GetApii<GetEsimHistoryResponse>(url);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }
            return result;
        }

        #endregion

        #region Get eSIM Install Details
        public async Task<Result<GetEsimInstallDetailReponseDTO>> GetEsimInstallDetailAsync(string reference)
        {
            var result = new Result<GetEsimInstallDetailReponseDTO>();

            if (string.IsNullOrWhiteSpace(reference))
            {
                result.Success = false;
                result.Message = BusinessManager.Missingreference;
                return result;
            }
            string url = $"{BusinessManager.BaseURL}/esims/assignments?reference={reference}";

            try
            {
                var response = await _consumeApi.GetApi<GetEsimInstallDetailReponseDTO>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimInstallDetailNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.EsimInstallDetail;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        #region  GetListBundlesappliedtoeSIM
        public async Task<Result<ListBundlesAppliedToESIMResponseDTO>> GetListBundlesappliedtoeSIMAsync(ListBundlesAppliedToESIMRequestDTO request)
        {
            var result = new Result<ListBundlesAppliedToESIMResponseDTO>();

            try
            {
                string url = $"{BusinessManager.BaseURL}/esims/{request.iccid}/bundles";

                if (request.includeUsed.HasValue)
                    url += $"?includeUsed={request.includeUsed.Value.ToString().ToLower()}" +
                           (!string.IsNullOrEmpty(request.limit?.ToString()) ? $"&limit={request.limit}" : "");
                else if (request.limit.HasValue)
                    url += $"?limit={request.limit}";

                var response = await _consumeApi.GetApi<ListBundlesAppliedToESIMResponseDTO>(url);

                if (response == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.BundleNotFound;
                    return result;
                }

                result.Success = true;
                result.Data = response;
                result.Message = BusinessManager.ListofEsimBundleFetched;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error retrieving bundles: {ex.Message}.";
            }

            return result;
        }


        #endregion

        #region List eSims
        public async Task<Result<IEnumerable<EsimsDTO>>> GetListofEsimsAsync(string loggedUser)
        {
            var result = new Result<IEnumerable<EsimsDTO>>();

            try
            {
                var esimList = await _db.Esims.AsNoTracking().Where(u => u.SubscriberId == loggedUser).Select(u => new EsimsDTO()
                {
                    Id = u.Id,
                    SubscriberId = loggedUser,
                    CustomerRef = u.CustomerRef,
                    Iccid = u.Iccid,
                    Physical = u.Physical == false ? false : true,
                    ActionDate = u.ActionDate,
                    AssignedDate = u.AssignedDate,
                    LastAction = u.LastAction,
                }).ToListAsync();

                result.Data = esimList;

                if (!result.Data.Any())
                {
                    result.Message = BusinessManager.EmptyEsimList;
                }

                result.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = (int)HttpStatusCode.InternalServerError;
                result.Data = null;
            }

            return result;
        }
        #endregion
        #region UpdateInventory
        private async Task<bool> UpdateInventory(string subscriberId, string bundle)
        {
            bool result = false;

            try
            {
                var bundleFromSubscriberInventory = await _db.SubscribersInventory.FirstOrDefaultAsync(u => u.Item == bundle && u.SubscriberId == Guid.Parse(subscriberId));

                if (bundleFromSubscriberInventory is not null)
                {
                    bundleFromSubscriberInventory.Quantity -= 1;

                    if (bundleFromSubscriberInventory.Quantity == 0)
                    {
                        _db.SubscribersInventory.Remove(bundleFromSubscriberInventory);

                    }

                    await _db.SaveChangesAsync();

                    result = true;
                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }
    }
    #endregion
}
