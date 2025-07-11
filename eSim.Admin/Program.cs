using eSim.Admin.Models;
using eSim.EF.Context;
using eSim.EF.Entities;
using eSim.Implementations.Services.Account;
using eSim.Implementations.Services.Admin.Esims;
using eSim.Implementations.Services.Admin.Inventory;
using eSim.Implementations.Services.Admin.Order;
using eSim.Implementations.Services.Client;
using eSim.Implementations.Services.Email;
using eSim.Implementations.Services.Middleware.Subscriber;
using eSim.Implementations.Services.SystemClaimRepo;
using eSim.Implementations.Services.Ticket;
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.Interfaces.Admin.Account;
using eSim.Infrastructure.Interfaces.Admin.Client;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.Interfaces.Admin.Esim;
using eSim.Infrastructure.Interfaces.Admin.Inventory;
using eSim.Infrastructure.Interfaces.Admin.Order;
using eSim.Infrastructure.Interfaces.Admin.Ticket;
using eSim.Infrastructure.Interfaces.Middleware;
using eSim.Infrastructure.Interfaces.SystemClaimRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

string connectionstring = builder.Configuration.GetConnectionString("AppDbConnection") ?? string.Empty;

if (string.IsNullOrWhiteSpace(connectionstring))
{
    Console.WriteLine("Unable to load the connection string from AppSetting.json");
}


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionstring));
builder.Services.AddTransient<ISubscriberService, SubscriberService>();

builder.Services.AddScoped<ISystemClaimService, SystemClaimService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IClient, ClientService>();
builder.Services.AddTransient<IClientSettings, ClientSettingsService>();
builder.Services.AddTransient<ITicket, TicketService>();
builder.Services.AddTransient<IInventory, InventoryService>();
builder.Services.AddTransient<IAdminOrder, AdminOrderService>();
builder.Services.AddTransient<IEsims, EsimService>();
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfiguration"));

builder.Services.
    AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        /// password options and other here!
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 3;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.SignIn.RequireConfirmedEmail = true;

    }).
    AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings  
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Account/Index";     //set the login path.  
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

/// Applying Authorize Attribute Globally

builder.Services.AddControllersWithViews(options =>
{

    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));

});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IAuthorizationPolicyProvider, DynamicAuthorizationPolicyProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
