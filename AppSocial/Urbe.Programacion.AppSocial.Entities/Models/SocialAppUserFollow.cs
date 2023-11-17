using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Interfaces;
using Urbe.Programacion.Shared.Entities.Internal;

namespace Urbe.Programacion.AppSocial.Entities.Models;

public class SocialAppUserLike : IEntity
{
    private readonly KeyedNavigation<Guid, SocialAppUser> usernav = new();
    private readonly KeyedNavigation<Snowflake, Post> postnav = new();

    public Guid UserWhoLikedThisId
    {
        get => usernav.Id;
        set => usernav.Id = value;
    }

    public SocialAppUser? UserWhoLikedThis
    {
        get => usernav.Entity;
        set => usernav.Entity = value;
    }

    public Snowflake PostId
    {
        get => postnav.Id;
        set => postnav.Id = value;
    }

    public Post? Post
    {
        get => postnav.Entity;
        set => postnav.Entity = value;
    }
}

public class SocialAppUserFollow : IKeyed<Guid>, IEntity
{
    private readonly KeyedNavigation<Guid, SocialAppUser> followernav = new();
    private readonly KeyedNavigation<Guid, SocialAppUser> followednav = new();

    public Guid Id { get; }

    public Guid FollowerId
    {
        get => followernav.Id;
        set => followernav.Id = value;
    }

    public SocialAppUser? Follower
    {
        get => followernav.Entity;
        set => followernav.Entity = value;
    }

    public Guid FollowedId
    {
        get => followednav.Id;
        set => followednav.Id = value;
    }

    public SocialAppUser? Followed
    {
        get => followednav.Entity;
        set => followednav.Entity = value;
    }
}
