using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Internal;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class UserSession : IEntity, IKeyed<RandomKey>, ISelfModelBuilder<UserSession>
{
    private readonly KeyedNavigation<Guid, User> UserNavigation = new();

    public RandomKeyId<UserSession> Id { get; init; }
    RandomKey IKeyed<RandomKey>.Id => Id.Value;

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

    public DateTimeOffset LastAccess { get; init; }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<UserSession> mb)
    {
        mb.HasKey(x => x.Id);
        mb.Property(x => x.Id).HasConversion(RandomKeyId<UserSession>.ValueConverter);
        mb.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired(true);
    }
}
