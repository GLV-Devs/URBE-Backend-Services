using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppSocial.Common;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices.DTOs.Requests;
using Urbe.Programacion.AppSocial.ModelServices.DTOs.Responses;
using Urbe.Programacion.Shared.Entities;
using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.AppSocial.ModelServices.Implementations;

[RegisterService(typeof(IEntityCRDRepository<Post, Snowflake, PostCreationModel>))]
[RegisterService(typeof(IPostRepository))]
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

        var post = new Post(
            Snowflake.New(),
            requester.Id,
            model.Content,
            requester.UserName!,
            DateTimeOffset.Now,
            model.InResponseTo is long irt ? new(irt) : null
        );
        context.Posts.Add(post);

        return ValueTask.FromResult(new SuccessResult<Post>(post));
    }

    //public override async ValueTask<IQueryable<object>?> GetViews(SocialAppUser? requester, IQueryable<Post>? query)
    //{
    //    if (query is null) return null;

    //    var posts = await query.ToListAsync();
    //    return posts.AsQueryable();
    //}

    public override ValueTask<IQueryable<object>?> GetViews(SocialAppUser? requester, IQueryable<Post>? query)
        => ValueTask.FromResult<IQueryable<object>?>((
            query is null
            ? null
            : requester is null
            ? query.AsNoTracking().Include(x => x.Poster).Where(x => x.Poster != null && x.Poster.Settings.HasFlag(UserSettings.AllowAnonymousPostViews))
            : query.AsNoTracking().Include(x => x.Poster).Where(x => x.Poster != null && (x.Poster.Id == requester.Id || x.Poster.Settings.HasFlag(UserSettings.AllowNonFollowerPostViews) || requester.FollowedUsers != null && requester.FollowedUsers.Contains(x.Poster)))
            )?.Select(x => new PostViewModel()
            {
                Content = x.Content,
                DatePosted = x.DatePosted,
                Id = x.Id.AsLong(),
                Poster = new UserViewModel()
                {
                    UserId = x.Poster!.Id,
                    Username = x.Poster!.UserName!,
                    ProfilePictureUrl = x.Poster!.ProfilePictureUrl,
                    Pronouns = x.Poster!.Pronouns
                },
                InResponseTo = x.InResponseToId == null ? null : x.InResponseToId.Value.AsLong(),
                PosterId = x.Poster!.Id,
                PosterThenUsername = x.PosterThenUsername,
                Responses = x.Responses != null ? x.Responses.Select(x => x.Id.AsLong()).ToHashSet() : null
            })
        );

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

    public async ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetPosts(user) : null;

    public async ValueTask<SocialAppUser> GetPoster(Post post)
        => post.Poster ?? (await userRepository.Find(post.PosterId))!;

    private async ValueTask<bool> CanView(SocialAppUser? requester, SocialAppUser poster)
        => requester?.Id.Equals(poster.Id) is true
        || (poster.Settings.HasFlag(UserSettings.AllowAnonymousViews) || requester is not null)
            && (poster.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || requester is not null && await userRepository.IsFollowing(requester, poster));
}
