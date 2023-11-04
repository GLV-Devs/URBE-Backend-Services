using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices.DTOs.Requests;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.ModelServices;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IPostRepository : IEntityCRDRepository<Post, Snowflake, PostCreationModel>
{
    public ValueTask<SocialAppUser> GetPoster(Post post);
    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester);
    public ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser? requester, SocialAppUser user);
}