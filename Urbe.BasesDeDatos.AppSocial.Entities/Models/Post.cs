using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Internal;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class Post : IKeyed<Snowflake>, IEntity, ISelfModelBuilder<Post>
{
    private readonly KeyedNavigation<Guid, SocialAppUser> UserNavigation = new();
    private readonly KeyedNavigation<Snowflake, Post> InResponseToNavigation = new();

    public Post(Snowflake id, Guid posterId, string content, string posterThenUsername, DateTimeOffset datePosted, Snowflake inResponseToId)
    {
        Id = id;
        PosterId = posterId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        PosterThenUsername = posterThenUsername ?? throw new ArgumentNullException(nameof(posterThenUsername));
        DatePosted = datePosted;
        InResponseToId = inResponseToId;
    }

    public Snowflake Id { get; init; }

    public SocialAppUser? Poster
    {
        get => UserNavigation.Entity;
        init => UserNavigation.Entity = value;
    }

    public Guid PosterId
    {
        get => UserNavigation.Id;
        init => UserNavigation.Id = value;
    }

    public string Content { get; init; }

    public string PosterThenUsername { get; init; }

    public DateTimeOffset DatePosted { get; init; }

    public Post? InResponseTo
    {
        get => InResponseToNavigation.Entity;
        init => InResponseToNavigation.Entity = value;
    }

    public Snowflake InResponseToId
    {
        get => InResponseToNavigation.Id;
        init => InResponseToNavigation.Id = value;
    }

    public HashSet<Post>? Responses { get; set; }

    Snowflake IKeyed<Snowflake>.Id => Id;

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<Post> mb)
    {
        mb.HasKey(x => x.Id);
        mb.Property(x => x.Id).HasConversion(Snowflake.ValueConverter);
        mb.HasOne(x => x.Poster).WithMany(x => x.Posts).HasForeignKey(x => x.PosterId).IsRequired(true);
        mb.HasOne(x => x.InResponseTo).WithMany(x => x.Responses).HasForeignKey(x => x.InResponseToId).IsRequired(false);
    }
}
