using System.Net;
using DiegoG.REST.ASPNET;
using DiegoG.REST.Json;
using Mail.NET;
using Mail.NET.MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Urbe.Programacion.AppSocial.WebApp.Server.Middleware;
using Urbe.Programacion.AppSocial.WebApp.Server.Options;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices.Implementations;
using Urbe.Programacion.Shared.API.Common.Filters;
using Urbe.Programacion.Shared.API.Common.Workers;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.ModelServices.Configuration;
using Urbe.Programacion.Shared.Services;
using Urbe.Programacion.Shared.API.Common.Services;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.API.Backend.Services;
using Urbe.Programacion.Shared.ModelServices.JsonConverters;
using Urbe.Programacion.AppSocial.DataTransfer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc.Cors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;
using Urbe.Programacion.AppSocial.WebApp.Server.Authentication;
using Urbe.Programacion.AppSocial.WebApp.Server.Services;
using Urbe.Programacion.AppSocial.WebApp.Server.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace Urbe.Programacion.AppSocial.WebApp.Server;
public static class Program
{
    public static WebApplication Application { get; }

    static Program()
    {
        var z = Environment.GetCommandLineArgs();
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        builder.Logging.AddSerilog();

        var services = builder.Services;
        builder.Configuration.AddJsonFile("appsettings.Secret.json", true);
        builder.Configuration.AddEnvironmentVariables();

        // Add services to the container.

        var corsconf = builder.Configuration.GetRequiredSection("CorsOrigins").Get<string[]>()
            ?? throw new InvalidDataException("CorsOrigins returned null");

        if (corsconf.Length is 0)
            throw new InvalidDataException("No CORS Origins configured");

        Log.Information("Configuring cors with the following origins: {origins}", corsconf);

        services.AddControllersWithViews();
        services.AddRazorPages();

        services.AddCors(options => options.AddDefaultPolicy(builder
            => builder
                .WithOrigins(corsconf)
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader()
                .WithExposedHeaders("Access-Control-Allow-Origin")
        ));

        services.ConfigureHttpJsonOptions(x => x.SerializerOptions.Converters.Add(SnowflakeConverter.Instance));

        services.AddControllers().AddOData(o => o.Select().Filter().OrderBy().Count().SetMaxTop(100));

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(o
            => o.AddSecurityDefinition("jwt", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Cookie,
                Description = "Please log in using the Identity controller",
                Name = "Json Web Token Bearer",
                Type = SecuritySchemeType.ApiKey
            }
        ));

        services.RegisterDecoratedOptions(builder.Configuration);

        services.AddScoped<IMailWriter>(x =>
        {
            var settings = x.GetRequiredService<IOptions<SmtpSettings>>().Value;
            return new MailKitSmtpWriter(settings.Domain, settings.Port, new Auth(settings.UserName, settings.Password), settings.UseSsl);
        });

        services.AddHostedService<BackgroundTaskStoreSweeper>();

        services.AddRESTObjectSerializer(
            x => new JsonRESTSerializer<SocialAPIResponseCode>(x.GetRequiredService<IOptions<JsonOptions>>().Value.SerializerOptions));

        services.AddMvc(o =>
        {
            o.Filters.Add<SignInRefreshFilter<SocialAppUser>>();
            o.Filters.Add(SocialAPIResponseFilter.SocialInstance);
        });

        services.RegisterDecoratedServices();

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
        .AddEntityFrameworkDbContextStores<SocialAppUser, SocialContext>();

        var jwto = builder.Configuration.GetRequiredSection("JWTOptions").Get<JWTOptions>();
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = true;
            //o.ClaimsIssuer = jwto?.Issuer;
            //o.Audience = jwto?.Audience;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Application!.Services.GetRequiredService<IOptions<JWTOptions>>().Value.SecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                //ValidIssuer = jwto?.Issuer,
                //ValidAudience = jwto?.Audience,
                //SignatureValidator = (t, p) => new JwtSecurityToken(t)
            };
        })
        .AddScheme<JwtSignInHandlerOptions, JwtSignInHandler>(
            "Identity.Application",
            o => o.TokenFactory = Application!.Services.GetRequiredService<JwtFactory>()
        );

        //services.ConfigureApplicationCookie(options =>
        //{
        //    // Cookie settings
        //    options.Cookie.Name = "SessionCookie";
        //    options.Cookie.HttpOnly = false;
        //    options.LoginPath = "/api/Identity/Contest";
        //    options.LogoutPath = "/api/Identity/Contest";
        //    options.ClaimsIssuer = "GarciaLozanoViloria-Urbe.Programacion.AppSocial.API";
        //    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        //    options.SlidingExpiration = true;
        //});

        services.UseAPIResponseInvalidModelStateResponse<SocialAPIResponseCode>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseCors();

        app.UseMiddleware<SocialAppAuthorizationMiddlewareResultHandler>();
        if (app.Environment.IsDevelopment())
        {
            app.UseVerboseExceptionHandler<SocialAPIResponseCode>();
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseHsts();
            app.UseObfuscatedExceptionHandler<SocialAPIResponseCode>();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");

        Application = app;
    }

    public static Task Main(string[] args)
        => Application.RunAsync();
}