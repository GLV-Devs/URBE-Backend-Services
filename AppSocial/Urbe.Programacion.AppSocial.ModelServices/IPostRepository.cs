using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.ModelServices;

public interface IPostRepository : IEntityCRDRepository<Post, Snowflake, PostCreationModel>
{
    public ValueTask<SocialAppUser> GetPoster(Post post);
    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester);
    public ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser? requester, SocialAppUser user);
}