using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IUserRepository : IEntityCRUDRepository<SocialAppUser, Guid, UserCreationModel, UserUpdateModel>
{
    public IQueryable<Post> GetPosts(SocialAppUser user);
    public IQueryable<SocialAppUser> GetFollowers(SocialAppUser user);
    public IQueryable<SocialAppUser> GetFollowing(SocialAppUser user);
    public IQueryable<SocialAppUser> GetMutuals(SocialAppUser user);
    public bool FollowUser(SocialAppUser follower, SocialAppUser followed);
}