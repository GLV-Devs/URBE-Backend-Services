using Urbe.BasesDeDatos.AppSocial.Services;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;
using Urbe.BasesDeDatos.AppSocial.Common;
using DiegoG.REST.ASPNET;
using System.Net;
using Urbe.BasesDeDatos.AppSocial.ModelServices.Configuration;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;
using Urbe.BasesDeDatos.AppSocial.API.Filters;
using DiegoG.REST.Json;
using Microsoft.AspNetCore.Http.Json;

namespace Urbe.BasesDeDatos.AppSocial.API;

public static class Program
{
    public static WebApplication Application { get; }

    static Program()
    {
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        var services = builder.Services;

        // Add services to the container.

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

        services.AddRESTObjectSerializer<APIResponseCode>(
            x => new JsonRESTSerializer<APIResponseCode>(x.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions));

        services.AddMvc(o => o.Filters.Add(APIResponseFilter.Instance));
        services.RegisterDecoratedServices();

        builder.Logging.AddSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        var dbconf = builder.Configuration.GetRequiredSection("DatabaseConfig").GetRequiredSection("SocialContext").Get<DatabaseConfiguration?>()
            ?? throw new InvalidDataException("SocialConfig parameter under DatabaseConfig section returned null");
        var connstr = DatabaseConfiguration.FormatConnectionString(builder.Configuration.GetConnectionString("SocialContext")!);

        if (dbconf.DatabaseType is DatabaseType.SQLServer)
        {
            Log.Information("Registering SocialContext backed by SQLServer");
            services.AddDbContext<SocialContext>(x => x.UseSqlServer(connstr));
        }
        else if (dbconf.DatabaseType is DatabaseType.SQLite)
        {
            Log.Information("Registering SocialContext backed by SQLite");
            var path = Regexes.SQLiteConnectionStringFilePath().Match(connstr).Groups[1].ValueSpan;
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(new string(dir));
            services.AddDbContext<SocialContext>(x => x.UseSqlite(connstr));
        }
        else
            throw new InvalidDataException($"Unknown Database Type: {dbconf.DatabaseType}");

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies(o => { });

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
        .AddEntityFrameworkStores<SocialContext>();

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = false;
            options.LoginPath = "/api/identity";
            options.LogoutPath = "/api/identity";
            options.ClaimsIssuer = "GarciaLozanoViloria-Urbe.BasesDeDatos.AppSocial.API";
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
