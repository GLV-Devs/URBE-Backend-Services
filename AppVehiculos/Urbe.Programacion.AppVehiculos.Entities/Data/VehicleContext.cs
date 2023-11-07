using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Urbe.Programacion.AppVehiculos.Entities.Data.Entities;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppVehiculos.Entities.Data;
public class VehicleContext : BaseAppContext
{
    public VehicleContext(DbContextOptions<VehicleContext> options)
        : base(options)
    { }

    public DbSet<VehicleUser> Users => Set<VehicleUser>();
    public DbSet<VehicleUserRole> UserRoles => Set<VehicleUserRole>();
    public DbSet<VehicleUserRoleAssignation> UserRoleAssignations => Set<VehicleUserRoleAssignation>();
    public DbSet<VehicleReport> Reports => Set<VehicleReport>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.BuildModel<VehicleUser>();
        mb.BuildModel<VehicleReport>();
        mb.BuildModel<VehicleUserRole>();

        var iuc = mb.Entity<IdentityUserClaim<Guid>>();
        iuc.HasKey(x => x.UserId);
        iuc.HasOne<VehicleUser>().WithMany().HasForeignKey(x => x.UserId);
    }
}
public class VehicleContextFactory : IDesignTimeDbContextFactory<VehicleContext>
{
    public VehicleContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<VehicleContext>();
        optionsBuilder.UseSqlServer();

        return new VehicleContext(optionsBuilder.Options);
    }
}