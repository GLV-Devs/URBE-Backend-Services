using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppVehiculos.WebApp.Data.Entities;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data;
public class VehicleContext : BaseAppContext
{
    public VehicleContext(DbContextOptions<VehicleContext> options)
        : base(options)
    { }

    public DbSet<VehicleUser> Users => Set<VehicleUser>();
    public DbSet<VehicleReport> Reports => Set<VehicleReport>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.BuildModel<VehicleUser>();
        mb.BuildModel<VehicleReport>();
    }
}
