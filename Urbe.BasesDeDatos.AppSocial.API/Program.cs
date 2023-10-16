using Urbe.BasesDeDatos.AppSocial.Services;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.Configuration;
using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;

namespace Urbe.BasesDeDatos.AppSocial.API;

public static class Program
{
    public static WebApplication Application { get; }

    static Program()
    {
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        // Add services to the container.

        builder.Services.AddControllers().AddOData(o => o.Select().Filter().OrderBy().Count().SetMaxTop(100));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(o 
            => o.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Cookie,
                Description = "Please log in using the Identity controller",
                Name = "Session",
                Type = SecuritySchemeType.ApiKey
            }
        ));

        builder.Services.RegisterDecoratedServices();

        builder.Logging.AddSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        var dbconf = builder.Configuration.GetRequiredSection("DatabaseConfig").GetValue<DatabaseConfiguration>("SocialContext");
        builder.Services.AddDbContext<SocialContext>(x =>
        {
            if (dbconf.DatabaseType is DatabaseType.SQLServer)
            {
                Log.Information("Registering SocialContext backed by SQLServer");
                x.UseSqlServer(builder.Configuration.GetConnectionString("SocialContext"));
            }
            else if (dbconf.DatabaseType is DatabaseType.SQLite)
            {
                Log.Information("Registering SocialContext backed by SQLite");
                x.UseSqlite(builder.Configuration.GetConnectionString("SocialContext"));
            }
        });

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies(o => { });

        builder.Services.AddIdentityCore<SocialAppUser>(o =>
        {
            o.Stores.MaxLengthForKeys = 128;

            o.SignIn.RequireConfirmedEmail = true;

            o.Password.RequireDigit = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequiredLength = 8;
            o.Password.RequiredUniqueChars = 5;

            o.Lockout.MaxFailedAccessAttempts = 3;

            o.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
            o.User.RequireUniqueEmail = true;
        })
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<SocialContext>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = false;
            options.LoginPath = "/api/identity";
            options.LogoutPath = "/api/identity";
            options.ClaimsIssuer = "GarciaLozanoViloria-Urbe.BasesDeDatos.AppSocial.API";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
            app.UseHsts();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        Application = app;
    }

    public static Task Main(string[] args)
        => Application.RunAsync();
}
