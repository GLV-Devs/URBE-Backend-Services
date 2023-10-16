using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IUserRepository : IEntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>
{
    public ValueTask<SocialAppUser?> FindByUsername(string username);

    public ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser followed);
    public ValueTask<IQueryable<SocialAppUser>> GetFollowers(SocialAppUser requester);
    public ValueTask<IQueryable<SocialAppUser>> GetFollowing(SocialAppUser requester);
    public ValueTask<IQueryable<SocialAppUser>> GetMutuals(SocialAppUser requester);

    public ValueTask<bool> IsFollowing(SocialAppUser requester, SocialAppUser follower, SocialAppUser followed);
    public ValueTask<IQueryable<SocialAppUser>?> GetFollowers(SocialAppUser? requester, SocialAppUser user);
    public ValueTask<IQueryable<SocialAppUser>?> GetFollowing(SocialAppUser? requester, SocialAppUser user);
    public ValueTask<IQueryable<SocialAppUser>?> GetMutuals(SocialAppUser? requester, SocialAppUser user);

    public ValueTask<bool> FollowUser(SocialAppUser requester, SocialAppUser followed);
}