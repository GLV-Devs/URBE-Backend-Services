using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IPostRepository : IEntityCRDRepository<Post, Snowflake, PostCreationModel> 
{
    public ValueTask<SocialAppUser> GetPoster(Post post);
    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester);
    public ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser requester, SocialAppUser user);
}