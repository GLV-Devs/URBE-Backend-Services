using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Internal;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class PendingMailConfirmation : IEntity, IKeyed<RandomKey>, ISelfModelBuilder<PendingMailConfirmation>
{
    private readonly KeyedNavigation<Guid, User> UserNavigation = new();

    public RandomKeyId<PendingMailConfirmation> Id { get; init; }

    public User? User
    {
        get => UserNavigation.Entity;
        init => UserNavigation.Entity = value;
    }

    public GuidId<User> UserId
    {
        get => UserNavigation.Id;
        init => UserNavigation.Id = value.Value;
    }

    public DateTimeOffset CreationDate { get; init; }

    RandomKey IKeyed<RandomKey>.Id => Id.Value;

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<PendingMailConfirmation> mb)
    {
        mb.HasKey(x => x.Id);
        mb.Property(x => x.Id).HasConversion(RandomKeyId<PendingMailConfirmation>.ValueConverter);
        mb.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired(true);
    }
}
