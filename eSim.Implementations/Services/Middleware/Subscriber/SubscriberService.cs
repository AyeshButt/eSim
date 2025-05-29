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
//using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eSim.Implementations.Services.Middleware.Subscriber
{
    public class SubscriberService : ISubscriberService
    {
        private readonly ApplicationDbContext _db;
       // private readonly IEmailService _emailService;

        // Constructor
        public SubscriberService(ApplicationDbContext db /*IEmailService email*/)
        {
            _db = db;
            //_emailService = email;
        }



        public async Task<Result<string>> EmailExists(string email)
        {
            var result = new Result<string>();

            if (string.IsNullOrEmpty(email))
            {
                result.Success = false;
                result.Message = "Email is required.";
                return result;
            }

            var exists = await _db.Subscribers.AnyAsync(x => x.Email == email);

            result.Success = true;
            result.Message = exists ? "Email already exists." : "Email is available.";
            return result;
        }


        public async Task<Result<string>> CreateSubscriber(SubscriberRequestDTO input)
        {
            var result = new Result<string>();


            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var client = await _db.Client.FirstOrDefaultAsync(c => c.Name == input.MerchantId);
                if (client == null)
                {
                    result.Success = false;
                    result.Message = "Invalid Merchant Details";
                    return result;
                }

                string hashedPassword = PasswordHasher.HashPassword(input.Password);

                var subscriber = new Subscribers
                {
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    Email = input.Email,
                    Hash = hashedPassword,
                    ClientId = client.Id,
                    Country = input.Country
                };

                await _db.Subscribers.AddAsync(subscriber);
                await _db.SaveChangesAsync();

                // Email bhejna
                var email = new EmailDTO
                {
                    To = input.Email,
                    Subject = "Welcome to eSim",
                    Body = $"Hi {input.FirstName},\n\nYou are successfully signed up on our platform.\n\nThanks,\neSim Team"
                };

               // var emailResult = _emailService.SendEmail(email);

                //if (!emailResult.Success)
                //{
                //    // Email fail hone par bhi transaction commit kar do, lekin warning bhej do
                //    await transaction.CommitAsync();
                //    result.Success = true;
                //    result.Message = "User created, but email sending failed.";
                //    return result;
                //}

                // Sab kuch theek ho to commit karo transaction
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "User created and email sent successfully.";
                return result;
            }
            catch (Exception ex)
            {
                // Exception aane par rollback karo transaction
                try
                {
                    await transaction.RollbackAsync();
                }
                catch
                {
                    // Rollback fail ho to isko ignore karo, warna asli error chhup jayega
                }

                result.Success = false;
                result.Message = "An error occurred: " + ex.Message;
                return result;
            }
        }


        public async Task<Result<string>> UpdateSubscriberAsync(Guid id, UpdateSubscriberDTO input)
        {
            var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var result = new Result<string>();
                var subscriber = await _db.Subscribers.FirstOrDefaultAsync(x => x.Id == id);

                if (subscriber is null)
                {
                    result.Success = false;
                    result.Message = "Subscriber not found";
                    return result;
                }

                subscriber.FirstName = input.FirstName;
                subscriber.LastName = input.LastName;
                subscriber.Country = input.Country;
                subscriber.ModifiedAt = BusinessManager.GetDateTimeNow();

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "Subscriber updated successfully";
                return result;
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

                // Save path to DTO
                dto.ProfileImage = $"/uploads/{fileName}";

                // Save to DB
                var subscriber = await _db.Subscribers.FindAsync(dto.SubscriberId);
                if (subscriber == null)
                    return new Result<string?> { Success = false, Message = "Subscriber not found." };

                subscriber.ProfileImage = dto.ProfileImage;
                subscriber.ModifiedAt = DateTime.UtcNow;

                await _db.SaveChangesAsync();

                return new Result<string?> { Success = true, Message = "Image uploaded and saved to database." };
            }
            catch (Exception)
            {
                return new Result<string?> { Success = false, Message = "Error uploading image." };
            }
        }



    }
}

