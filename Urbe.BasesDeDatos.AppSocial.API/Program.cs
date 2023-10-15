using Urbe.BasesDeDatos.AppSocial.Services;
using Serilog.Extensions.Hosting;
using Serilog.Extensions.Logging;
using Serilog;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.Configuration;
using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Urbe.BasesDeDatos.AppSocial.API;

public static class Program
{
    public static WebApplication Application { get; }

    static Program()
    {
        var builder = WebApplication.CreateBuilder(Environment.GetCommandLineArgs());

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        builder.Services.AddDefaultIdentity<SocialAppUser>()

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        Application = app;
    }

    public static Task Main(string[] args)
        => Application.RunAsync();
}
