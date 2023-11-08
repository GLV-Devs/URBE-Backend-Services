using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

public class VehicleUser : BaseAppUser, ISelfModelBuilder<VehicleUser>
{
    public HashSet<VehicleReport>? Reports { get; set; }

    public HashSet<VehicleUserRole>? Roles { get; set; }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<VehicleUser> mb, DbContext context)
    {
        mb.HasKey(x => x.Id);
        mb.HasIndex(x => x.UserName).IsUnique(true);
        mb.HasIndex(x => x.NormalizedUserName).IsUnique(true);
        mb.HasIndex(x => x.Email).IsUnique(true);
        mb.HasIndex(x => x.NormalizedEmail).IsUnique(true);

        mb.Property(x => x.LockoutEnd).DateTimeOffsetAsTicksIfSQLite(context);

        var rolemb = mb.HasMany(x => x.Roles).WithMany().UsingEntity<VehicleUserRoleAssignation>();
        rolemb.HasKey(x => x.Id);
        rolemb.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        rolemb.HasOne(x => x.UserRole).WithMany().HasForeignKey(x => x.UserRoleId);

        mb.Property(x => x.Email).HasMaxLength(EmailMaxLength);
        mb.Property(x => x.RealName).HasMaxLength(RealNameMaxLength);
        mb.Property(x => x.UserName).HasMaxLength(UserNameMaxLength).IsRequired(true);
        mb.Property(x => x.ConcurrencyStamp).IsConcurrencyToken(true);
    }
}
