using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eSim.Common.StaticClasses;
using eSim.EF.Context;
using eSim.Infrastructure.DTOs.Account;
using eSim.Infrastructure.DTOs.Global;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eSim.Implementations.Services.Middleware.Subscriber
{
    public class SubscriberService(ApplicationDbContext db) : ISubscriberService
    {
        private readonly ApplicationDbContext _db = db;


       

        public async Task<Result<string>> CreateSubscriber(SubscriberRequestDTO input)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
               
                var salt = BusinessManager.GenerateUniqueAlphanumericId(10);

                var client = await _db.Client.FirstOrDefaultAsync(a=>a.Name ==input.MerchantId);

                if (client is not null)
                {


                    string hashedPassword = PasswordHasher.HashPassword(input.Password);


                    await _db.Subscribers.AddAsync(new EF.Entities.Subscribers
                    {

                        Active = true,
                        CreatedAt = BusinessManager.GetDateTimeNow(),
                        ModifiedAt = BusinessManager.GetDateTimeNow(),
                        FirstName = input.FirstName,
                        LastName = input.LastName,
                        Email = input.Email,
                        Hash = hashedPassword,
                        ClientId = client.Id,
                        Country = input.Country




                    });

                    await _db.SaveChangesAsync();
                    var r = PasswordHasher.VerifyPassword(input.Password, hashedPassword);



                    await transaction.CommitAsync();
                    return new Result<string>()
                    {
                        Data = null
                    };
                }
                else
                {
                    return new Result<string>()
                    {

                        Data = "Invalid Merchant Details"

                    };
                }
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();

                return new Result<string>()
                {

                    Data = ex.Message,

                };

            }

        }

        public Task<bool> EmailExists(string email)
        {
            return _db.Subscribers.AnyAsync(x => x.Email == email);
        }

       

        public async Task<Result<string>> UpdateSubscriberAsync(Guid id, UpdateSubscriberDTO input)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(x => x.Id == id);

                if (subscriber is null)
                    return new Result<string> { Success = false, Data = "Subscriber not found" };

                subscriber.FirstName = input.FirstName;
                subscriber.LastName = input.LastName;
                subscriber.Country = input.Country;
                subscriber.ModifiedAt = BusinessManager.GetDateTimeNow();

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return new Result<string> { Success = true, Data = "Subscriber updated successfully" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Result<string> { Success = false, Data = ex.Message };
            }
        }

        public async Task<Result<string?>> UploadProfileImageAsync(IFormFile file, ProfileImageDTO dto)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return new Result<string?> { Success = false, Message = "No image file provided." };

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var ext = Path.GetExtension(file.FileName).ToLower();

                if (!allowedExtensions.Contains(ext))
                    return new Result<string?> { Success = false, Message = "Only image files allowed (.jpg, .png, .gif)." };

                if (file.Length > 5 * 1024 * 1024)
                    return new Result<string?> { Success = false, Message = "File size limit exceeded (5 MB max)." };

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                dto.ProfileImage = $"/uploads/{fileName}";

                return new Result<string?> { Success = true, Data = dto.ProfileImage, Message = "Image uploaded." };
            }
            catch (Exception)
            {
                return new Result<string?> { Success = false, Message = "Error uploading image." };
            }
        }


    }
}

    