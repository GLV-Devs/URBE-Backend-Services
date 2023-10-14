using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Internal;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class Post : IKeyed<Snowflake>, IEntity, ISelfModelBuilder<Post>, IReadable, IDeletable
{
    private readonly KeyedNavigation<Guid, User> UserNavigation = new();
    private readonly KeyedNavigation<Snowflake, Post> InResponseToNavigation = new();

    public Post(SnowflakeId<Post> id, User? poster, GuidId<User> posterId, string content, string posterThenUsername, DateTimeOffset datePosted, Post? inResponseTo, SnowflakeId<Post> inResponseToId)
    {
        Id = id;
        Poster = poster;
        PosterId = posterId;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        PosterThenUsername = posterThenUsername ?? throw new ArgumentNullException(nameof(posterThenUsername));
        DatePosted = datePosted;
        InResponseTo = inResponseTo;
        InResponseToId = inResponseToId;
    }

    public SnowflakeId<Post> Id { get; init; }

    public User? Poster
    {
        get => UserNavigation.Entity;
        init => UserNavigation.Entity = value;
    }

    public GuidId<User> PosterId
    {
        get => UserNavigation.Id;
        init => UserNavigation.Id = value.Value;
    }

    public string Content { get; init; }

    public string PosterThenUsername { get; init; }

    public DateTimeOffset DatePosted { get; init; }

    public Post? InResponseTo
    {
        get => InResponseToNavigation.Entity;
        init => InResponseToNavigation.Entity = value;
    }

    public SnowflakeId<Post> InResponseToId
    {
        get => InResponseToNavigation.Id;
        init => InResponseToNavigation.Id = value.Value;
    }

    public HashSet<Post>? Responses { get; set; }

    Snowflake IKeyed<Snowflake>.Id => Id.Value;

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<Post> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasOne(x => x.Poster).WithMany(x => x.Posts).HasForeignKey(x => x.PosterId).IsRequired(true);
        mb.HasOne(x => x.InResponseTo).WithMany(x => x.Responses).HasForeignKey(x => x.InResponseToId).IsRequired(false);

        mb.Property(x => x.InResponseToId).HasConversion(SnowflakeId<Post>.ValueConverter);
        mb.Property(x => x.Id).HasConversion(SnowflakeId<Post>.ValueConverter);
    }
}
