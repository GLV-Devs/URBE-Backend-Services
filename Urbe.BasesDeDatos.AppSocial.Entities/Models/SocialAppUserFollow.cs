using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Internal;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class SocialAppUserFollow : IKeyed<Guid>, IEntity
{
    private readonly KeyedNavigation<Guid, SocialAppUser> followernav = new();
    private readonly KeyedNavigation<Guid, SocialAppUser> followednav = new();

    public GuidId<SocialAppUserFollow> Id { get; }
    
    public GuidId<SocialAppUser> FollowerId 
    {
        get => followernav.Id;
        set => followernav.Id = value.Value;
    }

    public SocialAppUser? Follower
    {
        get => followernav.Entity;
        set => followernav.Entity = value;
    }

    public GuidId<SocialAppUser> FollowedId
    {
        get => followednav.Id;
        set => followednav.Id = value.Value;
    }

    public SocialAppUser? Followed
    {
        get => followednav.Entity;
        set => followednav.Entity = value;
    }

    Guid IKeyed<Guid>.Id => Id.Value;
}
