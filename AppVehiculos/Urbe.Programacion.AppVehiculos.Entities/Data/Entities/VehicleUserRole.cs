using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppVehiculos.Entities.Data.Entities;

public class VehicleUserRole : BaseAppRole, ISelfModelBuilder<VehicleUserRole>
{
    public const string AdminReportViewerRole = "ReportViewer";

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<VehicleUserRole> mb, DbContext context)
    {
        mb.HasKey(x => x.Id);
        mb.HasIndex(x => x.Name).IsUnique(true);
        mb.HasIndex(x => x.NormalizedName).IsUnique(true);
        mb.Property(x => x.ConcurrencyStamp).IsConcurrencyToken(true);
    }
}
