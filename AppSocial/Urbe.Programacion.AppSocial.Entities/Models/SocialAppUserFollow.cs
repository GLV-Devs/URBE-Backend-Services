using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.AppSocial.Entities.Interfaces;
using Urbe.Programacion.AppSocial.Entities.Internal;

namespace Urbe.Programacion.AppSocial.Entities.Models;

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
