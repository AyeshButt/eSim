﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.DTOs.Subscribers;

using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace eSim.Implementations.Services.Middleware.Subscriber
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _emailService;

        // Constructor
        public SubscriberService(ApplicationDbContext db, IEmailService email)
        {
            _db = db;
            _emailService = email;
        }

        public async Task<Result<string>> SubscriberEmailExists(string email)
        {
            var result = new Result<string>();


            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                result.Success = false;
                result.Message = BusinessManager.validemailaddress;
                result.StatusCode = StatusCodes.Status400BadRequest;
                return result;
            }

            var exists = await _db.Subscribers.AnyAsync(x => x.Email == email);
            if (exists)
            {
                result.Success = false;
                result.Message = BusinessManager.EmailExist;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }
            else
            {
                result.Success = true;
                result.Message = BusinessManager.EmailAvailable;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }
        }

        public async Task<Result<string>> CreateSubscriber(SubscriberDTORequest input)
        {
            var result = new Result<string>();

            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var client = await _db.Client.FirstOrDefaultAsync(c => c.Name == input.MerchantId);
                if (client == null || !Regex.IsMatch(input.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    result.Success = false;
                    result.Message = client == null ? BusinessManager.InvalidMerchant : BusinessManager.Invalidemailformat;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }
                if (!Regex.IsMatch(input.FirstName, @"^[A-Za-z]+$") || !Regex.IsMatch(input.LastName, @"^[A-Za-z]+$"))
                {
                    result.Success = false;
                    result.Message = !Regex.IsMatch(input.FirstName, @"^[A-Za-z]+$") ? BusinessManager.FirstName: BusinessManager.LastName;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }
                if (!Regex.IsMatch(input.Country, @"^[A-Za-z]+$"))
                {
                    result.Success = false;
                    result.Message = BusinessManager.InvalidCountryFormat;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }


                string hashedPassword = PasswordHasher.HashPassword(input.Password);

                var subscriber = new Subscribers
                {
                    Active = true,
                    CreatedAt = BusinessManager.GetDateTimeNow(),
                    ModifiedAt = BusinessManager.GetDateTimeNow(),
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Email = input.Email.ToLower(),
                    Hash = hashedPassword,
                    ClientId = client.Id,
                    Country = input.Country
                };

                await _db.Subscribers.AddAsync(subscriber);
                await _db.SaveChangesAsync();

                try
                {
                    var emailResult = _emailService.SendEmail(new EmailDTO
                    {
                        To = input.Email,
                        Subject = BusinessManager.SubscriberSubject,
                        Body = BusinessManager.GetSubscriberBody(input.FirstName)
                    });


                    await transaction.CommitAsync();

                    //     result.Message = emailResult.Success ? BusinessManager.EmailSendSuccessfully : BusinessManager.EmailNotSend;

                }
                catch (Exception ex)

                {
                    await transaction.RollbackAsync();
                    result.Success = false;
                    result.Message = ex.Message;
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    return result;

                }
                finally
                {
                    result.Message = BusinessManager.SubscriberCreatedSuccessfully;
                    result.StatusCode = StatusCodes.Status200OK;
                }


                return result;

            }
            catch (Exception ex)
            {

                try
                {
                    await transaction.RollbackAsync();
                }
                catch
                {
                    // Rollback fail ho to isko ignore karo, warna asli error chhup jayega
                }

                result.Success = false;
                result.Message = ex.Message;
                return result;
            }
        }


        public async Task<Result<string>> UpdateSubscriberAsync(Guid loggeduser, UpdateSubscriberDTORequest request)
        {
            var result = new Result<string>();
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(x => x.Id == loggeduser);

                if (subscriber is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.SubscriberNotFound;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;

                }
                if (!string.IsNullOrWhiteSpace(request.FirstName))
                {
                    if (!Regex.IsMatch(request.FirstName, @"^[A-Za-z]+$"))
                    {
                        result.Success = false;
                        result.Message = BusinessManager.FirstName;
                        result.StatusCode = StatusCodes.Status400BadRequest;
                        return result;
                    }
                    subscriber.FirstName = request.FirstName;
                }

                if (!string.IsNullOrWhiteSpace(request.LastName))
                {
                    if (!Regex.IsMatch(request.LastName, @"^[A-Za-z]+$"))
                    {
                        result.Success = false;
                        result.Message = BusinessManager.LastName;
                        result.StatusCode = StatusCodes.Status400BadRequest;
                        return result;
                    }
                    subscriber.LastName = request.LastName;
                }

                if (!string.IsNullOrWhiteSpace(request.Country))
                {
                    if (!Regex.IsMatch(request.Country, @"^[A-Za-z]+$"))
                    {
                        result.Success = false;
                        result.Message = BusinessManager.InvalidCountryFormat;
                        result.StatusCode = StatusCodes.Status400BadRequest;
                        return result;
                    }
                    subscriber.Country = request.Country;
                }
                subscriber.ModifiedAt = BusinessManager.GetDateTimeNow();

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = BusinessManager.Subscriberupdated;
                result.StatusCode = StatusCodes.Status200OK;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                {
                    result.Success = false;
                    result.Message = ex.Message;
                    result.StatusCode = StatusCodes.Status500InternalServerError;
                    return result;
                }

            }
            return result;
        }


        public async Task<Result<string?>>UploadProfileImageAsync(Guid loggeduser,IFormFile file, ProfileImageDTORequest dto)
        {

            var result = new Result<string?>();
            if (file == null || file.Length == 0)
            {

                {
                    result.Success = false;

                    result.Message = BusinessManager.Noimagefileprovided;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }
                ;
            }
            try
            {


                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(ext) || file.Length > 5 * 1024 * 1024)
                {
                    result.Success = false;
                    result.Message = !allowedExtensions.Contains(ext) ? BusinessManager.FileAllowed : BusinessManager.FileSize;
                    return result;
                }



                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                dto.ProfileImage = $"/uploads/{fileName}";

                var subscriber = await _db.Subscribers.FindAsync(loggeduser);
                if (subscriber == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Subscribernotfound;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;
                }

                subscriber.ProfileImage = dto.ProfileImage;
                subscriber.ModifiedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                result.Success = true;
                result.Message = BusinessManager.ImageUploaded;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
            }

            return result;
        }


        public async Task<IQueryable<SubscribersResponseDTO>> GetClient_SubscribersListByID(string id)
        {
            IQueryable<SubscribersResponseDTO> result = null;
            if(!string.IsNullOrWhiteSpace(id) && Guid.TryParse(id, out Guid parsedClientId))
            {
                var client_subscriber_list = (from s in _db.Subscribers
                                              join c in _db.Client on s.ClientId equals c.Id
                                              where s.ClientId == parsedClientId
                                              select new SubscribersResponseDTO()
                                              {
                                                  Id = s.Id,
                                                  FirstName = s.FirstName,
                                                  LastName = s.LastName,
                                                  Email = s.Email,
                                                  Hash = s.Hash,
                                                  Active = s.Active,
                                                  ClientId = s.ClientId,
                                                  CreatedAt = s.CreatedAt,
                                                  ModifiedAt = s.ModifiedAt,
                                                  ClientName  = c.Name
                                              }
                                 );
                return await Task.FromResult(client_subscriber_list);

            }

            return await Task.FromResult(result);
        }
        public async Task<IQueryable<SubscribersResponseDTO>> GetClient_SubscribersListAsync()
        {
            var client_subscriber_list = (from s in _db.Subscribers
                                          join c in _db.Client on s.ClientId equals c.Id
                                          select new SubscribersResponseDTO()
                                          {
                                              Id = s.Id,
                                              FirstName = s.FirstName,
                                              LastName = s.LastName,
                                              Email = s.Email,
                                              Hash = s.Hash,
                                              Active = s.Active,
                                              ClientId = s.ClientId,
                                              CreatedAt = s.CreatedAt,
                                              ModifiedAt = s.ModifiedAt,
                                              ClientName = c.Name
                                          }
                                 );
            return await Task.FromResult(client_subscriber_list);
        }


        public async Task<IQueryable<SubscriberDTO>> GetSubscribersListAsync()
        {
            var model = _db.Subscribers.AsNoTracking().Select(u => new SubscriberDTO()
            {

                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Hash = u.Hash,
                Active = u.Active,
                ClientId = u.ClientId,
                CreatedAt = u.CreatedAt,
                ModifiedAt = u.ModifiedAt,
            });

            return await Task.FromResult(model);
        }
        #region SubscriberDetail
        public async Task<Result<SubscriberDTO>> GetSubscriberDetailAsync(Guid loggedUser)
        {
            var result = new Result<SubscriberDTO?>();
            try
            { var subscriber = await _db.Subscribers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == loggedUser);

                if(subscriber is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Subscribernotfound;
                    result.StatusCode = StatusCodes.Status400BadRequest;
                    return result;

                }
                var dto = new SubscriberDTO
                {
                    Id = subscriber.Id,
                    FirstName = subscriber.FirstName,
                    LastName = subscriber.LastName,
                    Email = subscriber.Email,
                    Country = subscriber.Country,
                    ProfileImage = subscriber.ProfileImage,
                    ClientId=loggedUser,
                    CreatedAt=subscriber.CreatedAt,
                    ModifiedAt=subscriber.ModifiedAt,
                    IsEmailVerifired = subscriber.IsEmailVerifired,
                    TermsAndConditions = subscriber.TermsAndConditions
                };
                result.Data = dto;
                result.Success = true;
                result.Message = BusinessManager.Subscriberdetail;
                result.StatusCode = StatusCodes.Status200OK;
                return result;
            }
            catch (Exception ex) { 
                result.Success = false;
                result.Message = ex.Message;
                result.StatusCode = StatusCodes.Status500InternalServerError;
                return result;  
            }
        }

       

    }
    }
    #endregion


