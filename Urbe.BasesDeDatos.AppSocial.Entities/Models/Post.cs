using System.Numerics;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class Post : IKeyed<Snowflake>, IEntity
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

    Snowflake IKeyed<Snowflake>.Id => Id.Value;
}
