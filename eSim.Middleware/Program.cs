
using eSim.Infrastructure.Interfaces.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using eSim.Implementations.Services.Auth;
using eSim.Implementations.Services.Middleware;
using eSim.EF.Context;
using Microsoft.EntityFrameworkCore;
using eSim.Infrastructure.Interfaces.ConsumeApi;

using eSim.Implementations.Services.Middleware.Bundle;
using eSim.Common.StaticClasses;
using eSim.Implementations.Services.Middleware.Ticket;
using eSim.Infrastructure.Interfaces.Middleware.Ticket;
using eSim.Implementations.Services.Middleware.Subscriber;
using eSim.Implementations.Services.Email;
using eSim.Infrastructure.Interfaces.Admin.Email;
using eSim.Infrastructure.DTOs.Email;
using eSim.EF.Entities;
using Microsoft.AspNetCore.Identity;
using eSim.Implementations.Services.Middleware.Order;
using eSim.Infrastructure.Interfaces.Middleware.Order;
using eSim.Infrastructure.Interfaces.Middleware.Inventory;
using eSim.Implementations.Services.Middleware.Inventory;
using eSim.Infrastructure.Interfaces.Middleware.Esim;
using eSim.Implementations.Services.Esim;
using eSim.Middleware.Filters;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;



var builder = WebApplication.CreateBuilder(args);

// Retrieve connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AppDbConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.WriteLine("Unable to load the connection string from appsettings.json");
}


// Register DbContext with the retrieved connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("EmailConfiguration"));
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
    .RequireAuthenticatedUser()


    .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
    config.Filters.Add<ValidateModelFilter>();
});


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    //options.UseAllOfToExtendReferenceSchemas(); // optional but good for model clarity
    //options.SupportNonNullableReferenceTypes(); // recommended
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Esim API", Version = "v1" });

    // Add JWT Bearer Token Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",

        Type = SecuritySchemeType.ApiKey,
        //Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {your token}'"

    });

    // Add security requirement so that JWT is used for all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"   
                }
            },
            new string[] { }
        }
    });


});

//services registration
builder.Services.AddHttpClient();
builder.Services.AddTransient<IConsumeApi, ConsumeAPI>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IBundleService, BundleService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<ITicketServices, TicketService>();
builder.Services.AddTransient<ISubscriberService, SubscriberService>();
builder.Services.AddTransient<IForgotPassword, ForgotPasswordServices>();
builder.Services.AddTransient<IOrder, OrderService>();
builder.Services.AddTransient<IInventory, InventoryService>();
builder.Services.AddTransient<IEsimService, EsimService>();
builder.Services.AddTransient<IInventory, InventoryService>();



//  Enable CORS(allow for frontend)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
        };
    });
var app = builder.Build();
app.MapControllers();

app.UseStaticFiles();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.Use(async (context, next) =>
    {
        if (context.Request.Path.StartsWithSegments("/swagger/v1/swagger.json"))
        {
            context.Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            context.Response.Headers["Pragma"] = "no-cache";
        }
        await next();
    });
    app.UseSwagger();
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT API v1");
    //    options.OAuthClientId("swagger-ui");
    //    options.OAuthAppName("JWT API - Swagger");
    //    options.OAuthUsePkce();
    //    options.ConfigObject.AdditionalItems.Add("persistAuthorization", "true");

    //});
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Esim API");
    });
}
app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();



/////