using eSim.Common.StaticClasses;
using eSim.EF.Entities;
using eSim.Implementations.Services.Middleware.Subscriber;
using eSim.Implementations.Services.Selfcare.Authentication;
using eSim.Implementations.Services.Selfcare.Bundle;
using eSim.Implementations.Services.Selfcare.Esim;
using eSim.Implementations.Services.Selfcare.Inventory;
using eSim.Implementations.Services.Selfcare.Reference;
using eSim.Implementations.Services.Selfcare.Subscriber;
using eSim.Implementations.Services.Selfcare.Ticket;
using eSim.Infrastructure.Interfaces.ConsumeApi;
using eSim.Infrastructure.Interfaces.Selfcare.Authentication;
using eSim.Infrastructure.Interfaces.Selfcare.Bundles;
using eSim.Infrastructure.Interfaces.Selfcare.Esim;
using eSim.Infrastructure.Interfaces.Selfcare.Inventory;
using eSim.Infrastructure.Interfaces.Selfcare.Refrence;
using eSim.Infrastructure.Interfaces.Selfcare.Subscriber;
using eSim.Infrastructure.Interfaces.Selfcare.Ticket;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<IMiddlewareConsumeApi, MiddlewareConsumeApi>();
builder.Services.AddTransient<eSim.Infrastructure.Interfaces.Selfcare.Authentication.IAuthenticationService, eSim.Implementations.Services.Selfcare.Authentication.AuthenticationService>();
builder.Services.AddTransient<ITicketService, TicketService>();
builder.Services.AddTransient<ICountyService, CountryService>();
builder.Services.AddTransient<IBundleService, BundleService>();
builder.Services.AddTransient<IEsimService, EsimService>();
builder.Services.AddTransient<IInventoryService, InventoryService>();
builder.Services.AddTransient<ISubscriber, SubscriberServices>();
builder.Services.AddControllersWithViews();


//Configure Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {

        options.LoginPath = "/Authentication/SignIn";
        options.AccessDeniedPath = "/Authentication/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = false;

    });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
