using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppVehiculos.WebApp.Data.Entities;

public class VehicleUser : BaseAppUser, ISelfModelBuilder<VehicleUser>
{
    public HashSet<VehicleReport>? Reports { get; set; }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<VehicleUser> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasIndex(x => x.UserName).IsUnique(true);

        mb.Property(x => x.Email).HasMaxLength(EmailMaxLength);
        mb.Property(x => x.RealName).HasMaxLength(RealNameMaxLength);
        mb.Property(x => x.UserName).HasMaxLength(UserNameMaxLength).IsRequired(true);
        mb.Property(x => x.Pronouns).HasMaxLength(PronounsMaxLength);
    }
}
