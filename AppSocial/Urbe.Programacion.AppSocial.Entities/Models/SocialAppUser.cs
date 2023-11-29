using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.AppSocial.DataTransfer;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Internal;
using Urbe.Programacion.Shared.Entities.Models;

namespace Urbe.Programacion.AppSocial.Entities.Models;

public class SocialAppUser : BaseAppUser, ISelfModelBuilder<SocialAppUser>
{
    private readonly NullableKeyedNavigation<Snowflake, Post> LastSeenInFeedNav = new();
    public const int PronounsMaxLength = 30;

    public string? ProfileMessage { get; set; }

    public UserSettings Settings { get; set; }

    public HashSet<SocialAppUser>? Followers { get; set; }

    public HashSet<Post>? Posts { get; set; }

    public int FollowerCount { get; init; }

    public string? ProfilePictureUrl { get; set; }

    public string? Pronouns { get; set; }

    public Snowflake? LastSeenPostInFeedId
    {
        get => LastSeenInFeedNav.Id;
        set => LastSeenInFeedNav.Id = value;
    }

    public Post? LastSeenPostInFeed
    {
        get => LastSeenInFeedNav.Entity;
        set => LastSeenInFeedNav.Entity = value;
    }

    public override bool PhoneNumberConfirmed
    {
        get => true;
        set { }
    }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<SocialAppUser> mb, DbContext context)
    {
        mb.HasKey(x => x.Id);

        mb.HasIndex(x => x.UserName).IsUnique(true);
        mb.HasIndex(x => x.NormalizedUserName).IsUnique(true);
        mb.HasIndex(x => x.Email).IsUnique(true);
        mb.HasIndex(x => x.NormalizedEmail).IsUnique(true);

        mb.Property(x => x.Email).HasMaxLength(EmailMaxLength).IsRequired(true);
        mb.Property(x => x.NormalizedEmail).HasMaxLength(EmailMaxLength).IsRequired(true);
        
        mb.Property(x => x.UserName).HasMaxLength(UserNameMaxLength).IsRequired(true);
        mb.Property(x => x.NormalizedUserName).HasMaxLength(UserNameMaxLength).IsRequired(true);

        mb.Property(x => x.RealName).HasMaxLength(RealNameMaxLength);
        mb.Property(x => x.Pronouns).HasMaxLength(PronounsMaxLength);
        mb.Property(x => x.ProfilePictureUrl).HasMaxLength(ProfilePictureUrlMaxLength);
        mb.Property(x => x.ProfileMessage).HasMaxLength(ProfileMessageMaxLength);

        mb.HasOne(x => x.LastSeenPostInFeed).WithMany((string?)null).HasForeignKey(x => x.LastSeenPostInFeedId).IsRequired(false);

        mb.Property(x => x.Settings)
            .HasDefaultValue(UserSettings.AllowAnonymousViews |
                             UserSettings.AllowNonFollowerViews |
                             UserSettings.AllowAnonymousPostViews |
                             UserSettings.AllowNonFollowerPostViews
                            );

        var followmb = mb.HasMany(x => x.Followers).WithMany().UsingEntity<SocialAppUserFollow>(
            right => right.HasOne(x => x.Follower).WithMany().HasForeignKey(x => x.FollowerId),
            left => left.HasOne(x => x.Followed).WithMany().HasForeignKey(x => x.FollowedId)
        );

        //var followmb = mb.HasMany(x => x.FollowedUsers).WithMany().UsingEntity<SocialAppUserFollow>(
        //    right => right.HasOne(x => x.Followed).WithMany().HasForeignKey(x => x.FollowedId),
        //    left => left.HasOne(x => x.Follower).WithMany().HasForeignKey(x => x.FollowerId)
        //);

        followmb.HasKey(x => x.Id);
        followmb.ToTable(x => x.HasCheckConstraint("CK_SocialAppUserFollow_NoSelfFollow", "FollowedId <> FollowerId"));
    }
}
