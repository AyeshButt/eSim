using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Admin.Esim;
using eSim.Infrastructure.DTOs.Admin.Ticket;
using eSim.Infrastructure.DTOs.Esim;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Ticket;
using eSim.Infrastructure.Interfaces.Admin.Esim;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace eSim.Implementations.Services.Admin.Esims
{
    public class EsimService(ApplicationDbContext db, IConsumeApi consumeApi) : IEsims
    {
        
        private readonly ApplicationDbContext _db = db;
        private readonly IConsumeApi _consumeApi = consumeApi;

        #region list for specific suscriber
        public async Task<List<EsimsDTO>> GetAllAsync(string Id)
        {

            var EsimList = await _db.Esims
                .Where(o => o.SubscriberId == Id)
                .Select(o => new EsimsDTO()
                {
                    Iccid = o.Iccid,
                    CustomerRef = o.CustomerRef,
                    ActionDate = o.ActionDate,
                    LastAction = o.LastAction,
                    Physical = o.Physical,
                    AssignedDate = o.AssignedDate,
                }).ToListAsync();

            return EsimList;
        }
        #endregion

        #region eSIM list for all subscribers
        public async Task<IQueryable<EsimsList>> GetEsimListForAllSubscribersAsync()
        {

            var esimListRequest = (from e in _db.Esims
                                   join s in _db.Subscribers on e.SubscriberId equals s.Id.ToString()
                                   join c in _db.Client on s.ClientId equals c.Id
                                   select new EsimsList
                                   {
                                       SubscriberId = e.SubscriberId,
                                       SubscriberName = s.FirstName + " " + s.LastName,
                                       Iccid = e.Iccid,
                                       AssignedDate = e.AssignedDate,
                                       LastAction = e.LastAction,
                                       ActionDate = e.ActionDate,
                                       Physical = e.Physical,
                                       Client = s.ClientId.ToString(),
                                   });

            return await Task.FromResult(esimListRequest);
        }
        #endregion

        #region get esim Detail 
        public async Task<Result<GetEsimDetailsResponse>> GetEsimDetailsAsync(string iccid)
        {
            var result = new Result<GetEsimDetailsResponse>();

            string url = $"{BusinessManager.BaseURL}/esims/{iccid}";

            try
            {
                var esim = await _db.Esims.FirstOrDefaultAsync(u => u.Iccid == iccid);

                if (esim is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotFound;
                    result.StatusCode = StatusCodes.Status404NotFound;
                    result.Data = null;

                    return result;
                }

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

        #region  Get List Bundles applied to eSIM
        public async Task<Result<ListBundlesAppliedToEsimResponse>> GetListBundlesAppliedToEsimAsync(string iccid)
        {
            var result = new Result<ListBundlesAppliedToEsimResponse>();

            var request = new ListBundlesAppliedToEsimRequest()
            {
                IncludeUsed = true,
            };
            string url = CreateAppliedBundlesEsimURL(request, iccid);

            try
            {
                var esim = await _db.Esims.FirstOrDefaultAsync(u => u.Iccid == iccid);

                if (esim is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotFound;
                    result.StatusCode = StatusCodes.Status404NotFound;
                    result.Data = null;

                    return result;
                }

                result = await _consumeApi.GetApii<ListBundlesAppliedToEsimResponse>(url);
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

        #region Esim history

        public async Task<Result<GetEsimHistoryResponse>> GetEsimHistoryAsync(string iccid)
        {
            var result = new Result<GetEsimHistoryResponse>();
            string url = $"{BusinessManager.BaseURL}/esims/{iccid}/history";

            try
            {
                var esim = await _db.Esims.FirstOrDefaultAsync(u => u.Iccid == iccid);

                if (esim is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.EsimNotFound;
                    result.StatusCode = StatusCodes.Status404NotFound;
                    result.Data = null;

                    return result;
                }

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

        #region Private function to create applied bundles esim url
        private string CreateAppliedBundlesEsimURL(ListBundlesAppliedToEsimRequest input, string iccid)
        {
            string baseUrl = $"{BusinessManager.BaseURL}/esims/{iccid}/bundles";

            var queryParams = new List<string>();

            if (input.IncludeUsed is not null)
                queryParams.Add($"includeUsed={input.IncludeUsed}");

            if (input.Limit is not null)
                queryParams.Add($"limit={input.Limit}");

            return queryParams.Count > 0 ? $"{baseUrl}?{string.Join("&", queryParams)}" : baseUrl;
        }
        #endregion

        #region get Alltickets of a subscriber   
        public async Task<List<TicketResponseViewModel>> GetAllTicketAsync(string id)
        {
            var TicketsList = await _db.Ticket
                 .Where(t => t.CreatedBy == id)
                 .Select(t => new TicketResponseViewModel
                 {
                     TRN = t.TRN,
                     Subject = t.Subject,
                     status = t.Status,
                     CreatedAt = t.CreatedAt,
                     CreatedBy = t.CreatedBy,
                     ModifiedBy = t.ModifiedBy,
                     ModifiedAt = t.ModifiedAt,

                 }).ToListAsync();


            return TicketsList;
        }
        #endregion
    }
}
