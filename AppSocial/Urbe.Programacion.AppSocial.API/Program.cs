using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog;
using Urbe.Programacion.AppSocial.Entities;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppSocial.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Urbe.Programacion.AppSocial.Common;
using DiegoG.REST.ASPNET;
using System.Net;
using Urbe.Programacion.AppSocial.ModelServices.API.Responses;
using DiegoG.REST.Json;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Mail.NET;
using Mail.NET.MailKit;
using Urbe.Programacion.AppSocial.ModelServices.JsonConverters;
using Urbe.Programacion.AppSocial.ModelServices.Configuration;
using Urbe.Programacion.AppSocial.ModelServices.Implementations;
using Urbe.Programacion.AppSocial.ModelServices.API.Responses;
using Urbe.Programacion.AppSocial.Common;
using Urbe.Programacion.AppSocial.API.Filters;
using Urbe.Programacion.AppSocial.API.Options;
using Urbe.Programacion.AppSocial.API.Services;
using Urbe.Programacion.AppSocial.API.Workers;
using Urbe.Programacion.AppSocial.API.Middleware;
using Urbe.Programacion.Shared.Services;

namespace Urbe.Programacion.AppSocial.API;

public static class Program
{
    public static WebApplication Application { get; }

    static Program()
    {
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        var services = builder.Services;
        builder.Configuration.AddJsonFile("appsettings.Secret.json");

        // Add services to the container.

        services.ConfigureHttpJsonOptions(x => x.SerializerOptions.Converters.Add(SnowflakeConverter.Instance));
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, SocialAppAuthorizationMiddlewareResultHandler>();

        services.AddControllers().AddOData(o => o.Select().Filter().OrderBy().Count().SetMaxTop(100));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o
            => o.AddSecurityDefinition("apiKey", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Cookie,
                Description = "Please log in using the Identity controller",
                Name = "Session",
                Type = SecuritySchemeType.ApiKey
            }
        ));

        services.AddScoped<IMailWriter>(x =>
        {
            var settings = x.GetRequiredService<IOptions<SmtpSettings>>().Value;
            return new MailKitSmtpWriter(settings.Domain, settings.Port, new Auth(settings.UserName, settings.Password), settings.UseSsl);
        });

        services.AddOptions<UserVerificationServiceOptions>();
        services.AddOptions<SmtpSettings>(); // These should be stored in appsettings.Secret.json

        services.AddHostedService<BackgroundTaskStoreSweeper>();

        services.AddRESTObjectSerializer(
            x => new JsonRESTSerializer<APIResponseCode>(x.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions));

        services.AddMvc(o =>
        {
            o.Filters.Add(APIResponseFilter.Instance);
            o.Filters.Add<SignInRefreshFilter>();
        });

        services.RegisterDecoratedServices();

        builder.Logging.AddSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        var dbconf = builder.Configuration.GetRequiredSection("DatabaseConfig").GetRequiredSection("SocialContext").Get<DatabaseConfiguration?>()
            ?? throw new InvalidDataException("SocialConfig parameter under DatabaseConfig section returned null");

        if (dbconf.DatabaseType is DatabaseType.SQLServer)
        {
            Log.Information("Registering SocialContext backed by SQLServer");
            services.AddDbContext<SocialContext>(x => x.UseSqlServer(
                dbconf.SQLServerConnectionString,
                o => o.MigrationsAssembly("Urbe.Programacion.AppSocial.Entities.SQLServerMigrations")
            ));
        }
        else if (dbconf.DatabaseType is DatabaseType.SQLite)
        {
            Log.Information("Registering SocialContext backed by SQLite");
            var conns = DatabaseConfiguration.FormatConnectionString(dbconf.SQLiteConnectionString);
            var path = Regexes.SQLiteConnectionStringFilePath().Match(conns).Groups[1].ValueSpan;
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(new string(dir));
            services.AddDbContext<SocialContext>(x => x.UseSqlite(
                conns,
                o => o.MigrationsAssembly("Urbe.Programacion.AppSocial.Entities.SQLiteMigrations")
            ));
        }
        else
            throw new InvalidDataException($"Unknown Database Type: {dbconf.DatabaseType}");

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        services.AddIdentityCore<SocialAppUser>(o =>
        {
            o.Stores.MaxLengthForKeys = 128;

            o.SignIn.RequireConfirmedEmail = false;
            o.SignIn.RequireConfirmedPhoneNumber = false;

            o.Password.RequireDigit = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequiredLength = 6;
            o.Password.RequiredUniqueChars = 4;

            o.Lockout.MaxFailedAccessAttempts = 3;

            o.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
            o.User.RequireUniqueEmail = true;
        })
        .AddSignInManager()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkSocialContextStores();

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.Name = "SessionCookie";
            options.Cookie.HttpOnly = false;
            options.LoginPath = "/api/identity";
            options.LogoutPath = "/api/identity";
            options.ClaimsIssuer = "GarciaLozanoViloria-Urbe.Programacion.AppSocial.API";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
        });

        services.UseRESTInvalidModelStateResponse(
            x => new RESTObjectResult<APIResponseCode>(new APIResponse(APIResponseCodeEnum.ErrorCollection)
            {
                Data = null,
                Errors = null
            })
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseRESTExceptionHandler((r, e, s, c) => Task.FromResult(new ExceptionRESTResponse<APIResponseCode>(
                new APIResponse(APIResponseCodeEnum.Exception)
                {
                    Exception = e?.ToString() ?? "Unknown error"
                },
                HttpStatusCode.InternalServerError
            )));
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts();

            app.UseRESTExceptionHandler((r, e, s, c) =>
            {
                var errors = new ErrorList();
                errors.AddError(ErrorMessages.InternalError());
                return Task.FromResult(new ExceptionRESTResponse<APIResponseCode>(
                    new APIResponse(APIResponseCodeEnum.ErrorCollection)
                    {
                        Errors = errors
                    },
                    HttpStatusCode.InternalServerError
                ));
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        Application = app;
    }

    public static Task Main(string[] args)
        => Application.RunAsync();
}
