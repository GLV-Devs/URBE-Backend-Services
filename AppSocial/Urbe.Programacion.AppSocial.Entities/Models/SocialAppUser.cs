using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppSocial.Entities.Models;

public class SocialAppUser : BaseAppUser
{
    public const int PronounsMaxLength = 30;

    public string? ProfileMessage { get; set; }

    public UserSettings Settings { get; set; }

    public HashSet<SocialAppUser>? FollowedUsers { get; set; }

    public HashSet<Post>? Posts { get; set; }

    public int FollowerCount { get; init; }

    public string? ProfilePictureUrl { get; set; }

    public string? Pronouns { get; set; }

    public override bool PhoneNumberConfirmed
    {
        get => true;
        set { }
    }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<SocialAppUser> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasIndex(x => x.UserName).IsUnique(true);

        mb.Property(x => x.Email).HasMaxLength(EmailMaxLength);
        mb.Property(x => x.RealName).HasMaxLength(RealNameMaxLength);
        mb.Property(x => x.UserName).HasMaxLength(UserNameMaxLength).IsRequired(true);
        mb.Property(x => x.Pronouns).HasMaxLength(PronounsMaxLength);
        mb.Property(x => x.ProfilePictureUrl).HasMaxLength(ProfilePictureUrlMaxLength);
        mb.Property(x => x.ProfileMessage).HasMaxLength(ProfileMessageMaxLength);

        mb.Property(x => x.Settings)
            .HasDefaultValue(UserSettings.AllowAnonymousViews |
                             UserSettings.AllowNonFollowerViews |
                             UserSettings.AllowAnonymousPostViews |
                             UserSettings.AllowNonFollowerPostViews
                            );

        var followmb = mb.HasMany(x => x.FollowedUsers).WithMany().UsingEntity<SocialAppUserFollow>(
            right => right.HasOne(x => x.Followed).WithMany().HasForeignKey(x => x.FollowedId),
            left => left.HasOne(x => x.Follower).WithMany().HasForeignKey(x => x.FollowerId)
        );

        followmb.HasKey(x => x.Id);
    }
}
