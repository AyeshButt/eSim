using eSim.EF.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
        public DbSet<GlobalSetting> GlobalSetting { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public DbSet<TicketAttachments> TicketAttachments { get; set; }
        public DbSet<TicketActivities> TicketActivities { get; set; }
        public DbSet<TicketStatus> TicketStatus { get; set; }
        public DbSet<TicketType> TicketType { get; set; }
        public DbSet<AspNetUsersType> AspNetUsersType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ClientSettings>(entity =>
            {
                entity.HasOne(e => e.Client)
                      .WithMany()
                      .HasForeignKey(e => e.ClientId);
            });

            modelBuilder.Entity<TicketType>().HasData(
               new TicketType { Id = 1, Type = "bundle" },
               new TicketType { Id = 2, Type = "payment" }
           );
            modelBuilder.Entity<TicketStatus>().HasData(
               new TicketStatus { Id = 1, Status = "open" },
               new TicketStatus { Id = 2, Status = "close" },
               new TicketStatus { Id = 3, Status = "in-progress" },
               new TicketStatus { Id = 4, Status = "waiting for reply" }

           );
            modelBuilder.Entity<TicketAttachmentType>().HasData(
               new TicketAttachmentType { Id = 1, AttachmentType = "Internal" },
               new TicketAttachmentType { Id = 2, AttachmentType = "External" }
           );
            modelBuilder.Entity<TicketCommentType>().HasData(
               new TicketCommentType { Id = 1, CommentType = "Customer" },
               new TicketCommentType { Id = 2, CommentType = "Admin" }

           );
                        modelBuilder.Entity<AspNetUsersType>().HasData(
               new AspNetUsersType { Id = 1, Type = "Developer" },
               new AspNetUsersType { Id = 2, Type = "Superadmin" },
               new AspNetUsersType { Id = 3, Type = "Subadmin" },
               new AspNetUsersType { Id = 4, Type = "Client" },
               new AspNetUsersType { Id = 5, Type = "Subclient" }

);
        }
    }
}
