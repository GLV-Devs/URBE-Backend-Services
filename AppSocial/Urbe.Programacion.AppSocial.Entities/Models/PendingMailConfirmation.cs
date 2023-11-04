using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.AppSocial.Entities.Interfaces;
using Urbe.Programacion.AppSocial.Entities.Internal;

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

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<PendingMailConfirmation> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasOne(x => x.User).WithOne().HasForeignKey<PendingMailConfirmation>(x => x.UserId).IsRequired(true);
        mb.HasIndex(x => x.Token).IsUnique(true);
        mb.Property(x => x.Id).HasConversion(RandomKey.ValueConverter);
    }
}
