using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices.DTOs.Requests;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IPostRepository : IEntityCRDRepository<Post, Snowflake, PostCreationModel>
{
    public ValueTask<SocialAppUser> GetPoster(Post post);
    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester);
    public ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser? requester, SocialAppUser user);
}