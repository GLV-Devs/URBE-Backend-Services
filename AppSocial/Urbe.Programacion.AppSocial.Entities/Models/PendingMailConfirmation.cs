using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Internal;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppSocial.Entities.Models;

public class PendingMailConfirmation : IEntity, IKeyed<RandomKey>, ISelfModelBuilder<PendingMailConfirmation>
{
    private readonly KeyedNavigation<Guid, SocialAppUser> UserNavigation = new();

    public required RandomKey Id { get; init; }

    public required string Token { get; init; }

    public SocialAppUser? User
    {
        get => UserNavigation.Entity;
        init => UserNavigation.Entity = value;
    }

    public Guid UserId
    {
        get => UserNavigation.Id;
        init => UserNavigation.Id = value;
    }

    public DateTimeOffset CreationDate { get; init; }

    public TimeSpan Validity { get; init; }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<PendingMailConfirmation> mb, DbContext context)
    {
        mb.HasKey(x => x.Id);
        mb.HasOne(x => x.User).WithOne().HasForeignKey<PendingMailConfirmation>(x => x.UserId).IsRequired(true);
        mb.HasIndex(x => x.Token).IsUnique(true);
        mb.Property(x => x.Id).HasConversion(Conversions.RandomKeyValueConverter);
    }
}
