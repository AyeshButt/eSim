using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public SubscriberService(ApplicationDbContext db ,IEmailService email)
        {
            _db = db;
             _emailService = email;
        }

        public async Task<Result<string>> SubscriberEmailExists(string email)
        {
            var result = new Result<string>();

            if (string.IsNullOrEmpty(email))
            {
                result.Success = false;
                result.Message = BusinessManager.EmailRequired;
                return result;
            }

            var exists = await _db.Subscribers.AnyAsync(x => x.Email == email);

            result.Success = true;
            result.Message = exists ? BusinessManager.EmailExist : BusinessManager.EmailAvailable;
            return result;
        }
        public async Task<Result<string>> CreateSubscriber(SubscriberDTORequest input)
        {
            var result = new Result<string>();

            await using var transaction = await _db.Database.BeginTransactionAsync();
            
            try
            {
                var client = await _db.Client.FirstOrDefaultAsync(c => c.Name == input.MerchantId);
            
                if (client == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.InvalidMerchant;
                    return result;
                }

                string hashedPassword = PasswordHasher.HashPassword(input.Password);

                var subscriber = new Subscribers
                {
                    Active = true,
                    CreatedAt =BusinessManager.GetDateTimeNow(),
                    ModifiedAt = BusinessManager.GetDateTimeNow(),
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Email = input.Email,
                    Hash = hashedPassword,
                    ClientId = client.Id,
                    Country = input.Country
                };

                await _db.Subscribers.AddAsync(subscriber);
                await _db.SaveChangesAsync();
             
                try {
                    var emailResult = _emailService.SendEmail( new EmailDTO
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
                    result.Message = $"Email send failed: {ex.Message}";
                    return result;

                }
                finally
                {
                    result.Message = BusinessManager.SubscriberCreatedSuccessfully;
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
        

        public async Task<Result<string>> UpdateSubscriberAsync(Guid id, UpdateSubscriberDTORequest request)
        {
            var result = new Result<string>();
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
             
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(x => x.Id == id);

                if (subscriber is null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.SubscriberNotFound;
                   
                }

                subscriber.FirstName = request.FirstName;
                subscriber.LastName = request.LastName;
                subscriber.Country = request.Country;
                subscriber.ModifiedAt = BusinessManager.GetDateTimeNow();

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = BusinessManager.Subscriberupdated;
           
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Result<string> { Success = false, Message = ex.Message };
            }
           return result;
        }


        public async Task<Result<string?>> 
            
            UploadProfileImageAsync(IFormFile file, ProfileImageDTORequest dto)
        {
           
            var result = new Result<string?>();
            if (file == null || file.Length == 0)
            {
              
                {
                    result.Success = false;
                     result.Message =BusinessManager.Noimagefileprovided;
                    return result;
                };
            }
            try
            {
                if (file == null || file.Length == 0)
                {
                    result.Success = false;
                    result.Message = BusinessManager.NoFileProvided;
                    return result;
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(ext)|| file.Length > 5 * 1024 * 1024)
                {
                    result.Success = false;
                    result.Message = !allowedExtensions.Contains(ext)  ? BusinessManager.FileAllowed: BusinessManager.FileSize;
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

                var subscriber = await _db.Subscribers.FindAsync(dto.SubscriberId);
                if (subscriber == null)
                {
                    result.Success = false;
                    result.Message = BusinessManager.Subscribernotfound;
                    return result;
                }

                subscriber.ProfileImage = dto.ProfileImage;
                subscriber.ModifiedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                result.Success = true;
                result.Message = BusinessManager.ImageUploaded;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message =ex.Message ;
            }

            return result;
        }


        public async Task<IQueryable<SubscriberDTO>> GetClient_SubscribersListAsync(string id)
        {
            Guid parsedClientId = Guid.Parse(id);

            var client_subscriber_list = (from s in _db.Subscribers
                                          join c in _db.Client on s.ClientId equals c.Id
                                          where s.ClientId == parsedClientId
                                          select new SubscriberDTO()
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
    }
}

