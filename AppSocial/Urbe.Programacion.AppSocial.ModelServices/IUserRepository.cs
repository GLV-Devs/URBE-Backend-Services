using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.ModelServices;

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
    public ValueTask<SuccessResult<object>> GetSelfView(SocialAppUser user);
    public ValueTask<bool> FollowUser(SocialAppUser requester, SocialAppUser followed);

    public ValueTask<bool> UnfollowUser(SocialAppUser requester, SocialAppUser followed);
}