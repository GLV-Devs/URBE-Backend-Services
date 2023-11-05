using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Urbe.Programacion.AppVehiculos.WebApp.Data;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Entities;
using Urbe.Programacion.Shared.API.Common.Filters;
using Urbe.Programacion.Shared.API.Common.Services;
using Urbe.Programacion.Shared.API.Common.Workers;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.ModelServices.Configuration;
using Urbe.Programacion.Shared.ModelServices.JsonConverters;
using Urbe.Programacion.Shared.Services;

namespace Urbe.Programacion.AppVehiculos.WebApp;
public static class Program
{
    public static WebApplication Application { get; }

    static Program()
    {
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        var services = builder.Services;
        builder.Configuration.AddJsonFile("appsettings.Secret.json", true);

        // Add services to the container.

        services.ConfigureHttpJsonOptions(x => x.SerializerOptions.Converters.Add(SnowflakeConverter.Instance));

        services.AddHostedService<BackgroundTaskStoreSweeper>();

        services.AddMvc(o => o.Filters.Add<SignInRefreshFilter<VehicleUser>>());

        services.RegisterDecoratedServices();

        builder.Logging.AddSerilog();

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();

        var dbconf = builder.Configuration.GetRequiredSection("DatabaseConfig").GetRequiredSection("VehicleContext").Get<DatabaseConfiguration?>()
            ?? throw new InvalidDataException("SocialConfig parameter under DatabaseConfig section returned null");

        if (dbconf.DatabaseType is DatabaseType.SQLServer)
        {
            Log.Information("Registering VehicleContext backed by SQLServer");
            services.AddDbContext<VehicleContext>(x => x.UseSqlServer(
                dbconf.SQLServerConnectionString,
                o => o.MigrationsAssembly("Urbe.Programacion.AppVehiculos.Entities.SQLServerMigrations")
            ));
        }
        else if (dbconf.DatabaseType is DatabaseType.SQLite)
        {
            Log.Information("Registering VehicleContext backed by SQLite");
            var conns = DatabaseConfiguration.FormatConnectionString(dbconf.SQLiteConnectionString, "AppVehiculos");
            var path = Regexes.SQLiteConnectionStringFilePath().Match(conns).Groups[1].ValueSpan;
            var dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(new string(dir));
            services.AddDbContext<VehicleContext>(x => x.UseSqlite(
                conns,
                o => o.MigrationsAssembly("Urbe.Programacion.AppVehiculos.Entities.SQLiteMigrations")
            ));
        }
        else
            throw new InvalidDataException($"Unknown Database Type: {dbconf.DatabaseType}");

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

        services.AddIdentityCore<VehicleUser>(o =>
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
        .AddEntityFrameworkDbContextStores<VehicleUser, VehicleContext>();

        services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.Name = "SessionCookie";
            options.Cookie.HttpOnly = false;
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.ClaimsIssuer = "DiegoGarcia-Urbe.Programacion.AppVehiculos.API";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        Application = app;
    }

    public static Task Main(string[] args)
        => Application.RunAsync();
}
