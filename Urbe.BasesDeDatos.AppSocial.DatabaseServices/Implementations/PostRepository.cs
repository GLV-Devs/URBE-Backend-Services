using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;

public class PostRepository : EntityCRDRepository<Post, Snowflake, PostCreationModel>, IPostRepository
{
    private readonly IUserRepository userRepository;
    public PostRepository(IUserRepository userRepository, SocialContext context, IServiceProvider provider) : base(context, provider)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    public override ValueTask<SuccessResult<Post>> Create(SocialAppUser? requester, PostCreationModel model)
    {
        var errors = new ErrorList();

        if (requester is null)
        {
            errors.AddError(ErrorMessages.NoPermission());
            return ValueTask.FromResult(new SuccessResult<Post>(errors));
        }

        if (string.IsNullOrWhiteSpace(model.Content))
        {
            errors.AddError(ErrorMessages.NoPostContent());
            return ValueTask.FromResult(new SuccessResult<Post>(errors));
        }

        return ValueTask.FromResult(new SuccessResult<Post>(new Post(
            Snowflake.New(),
            requester,
            requester.Id,
            model.Content,
            requester.UserName!,
            DateTimeOffset.Now,
            null,
            model.InResponseTo ?? default
        )));
    }

    public override async ValueTask<SuccessResult<object>> GetView(SocialAppUser? requester, Post entity)
    {
        var errorlist = new ErrorList();

        if (await CanView(requester, await GetPoster(entity)))
            return new SuccessResult<object>(PostViewModel.FromPost(entity));
        else
        {
            errorlist.AddError(ErrorMessages.NoPermission());
            return new SuccessResult<object>(errorlist);
        }
    }

    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester)
        => ValueTask.FromResult(context.Posts.Where(x => x.PosterId == requester.Id));

    public async ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetPosts(user) : null;

    public async ValueTask<SocialAppUser> GetPoster(Post post)
        => post.Poster ?? (await userRepository.Find(post.PosterId.Value))!;

    private async ValueTask<bool> CanView(SocialAppUser? requester, SocialAppUser poster)
        => (requester?.Id.Equals(poster.Id) is true)
        || ((poster.Settings.HasFlag(UserSettings.AllowAnonymousViews) || requester is not null)
            && (poster.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || (requester is not null && await userRepository.IsFollowing(requester, poster))));
}
