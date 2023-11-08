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
        mb.BuildModel<VehicleUser>(this);
        mb.BuildModel<VehicleReport>(this);
        mb.BuildModel<VehicleUserRole>(this);

        var iuc = mb.Entity<IdentityUserClaim<Guid>>();
        iuc.HasKey(x => x.UserId);
        iuc.HasOne<VehicleUser>().WithMany().HasForeignKey(x => x.UserId);
    }
}