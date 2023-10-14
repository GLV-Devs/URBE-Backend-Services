using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;

public class PostRepository : EntityCRDRepository<Post, Snowflake, PostCreationModel>, IPostRepository
{
    public PostRepository(SocialContext context, IServiceProvider provider) : base(context, provider) { }

    public override ValueTask<SuccessResult<Post>> Create(SocialAppUser? requester, PostCreationModel model)
    {
        throw new NotImplementedException();
    }

    public override ValueTask<SuccessResult<object>> GetView(SocialAppUser requester, Post entity)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester)
    {
        throw new NotImplementedException();
    }

    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester, SocialAppUser user)
    {
        throw new NotImplementedException();
    }
}
