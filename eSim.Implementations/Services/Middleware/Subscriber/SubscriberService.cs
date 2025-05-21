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
using Microsoft.AspNetCore.Identity;
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
    }
}

    