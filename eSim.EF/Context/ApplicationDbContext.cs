using eSim.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSim.EF.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<SystemClaims> SystemClaims { get; set; }
        public DbSet<SideMenu> SideMenu { get; set; }
        public DbSet<OTPVerification> OTPVerification { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientSettings> ClientSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ClientSettings>(entity =>
            {
                entity.HasOne(e => e.Client)
                      .WithMany()
                      .HasForeignKey(e => e.ClientId);
            });


        }
    }
}
