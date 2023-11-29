using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.Entities.Models;
using Urbe.Programacion.Shared.ModelServices;
using Urbe.Programacion.Shared.ModelServices.Implementations;
using Urbe.Programacion.Shared.Services.Attributes;
using Urbe.Programacion.AppSocial.DataTransfer;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Urbe.Programacion.AppSocial.ModelServices.Implementations;

[RegisterService(typeof(IEntityCRDRepository<Post, Snowflake, PostCreationModel>))]
[RegisterService(typeof(IPostRepository))]
public class PostRepository : EntityCRDRepository<Post, Snowflake, PostCreationModel>, IPostRepository
{
    private readonly IUserRepository userRepository;
    private new readonly SocialContext context;

    public PostRepository(IUserRepository userRepository, SocialContext context, IServiceProvider provider) : base(context, provider)
    {
        this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        this.context = context;
    }

    public override ValueTask<SuccessResult> Delete(BaseAppUser? requester, Post entity)
    {
        if (requester is not null && requester.Id == entity.PosterId)
            return base.Delete(requester, entity);

        ErrorList x = new();
        x.AddError(ErrorMessages.NoPermission());
        x.RecommendedCode = System.Net.HttpStatusCode.Unauthorized;
        return ValueTask.FromResult(new SuccessResult(x));
    }

    public override ValueTask<SuccessResult<Post>> Create(BaseAppUser? requester, PostCreationModel model)
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
        context.Set<Post>().Add(post);

        return ValueTask.FromResult(new SuccessResult<Post>(post));
    }

    //public override async ValueTask<IQueryable<object>?> GetViews(SocialAppUser? requester, IQueryable<Post>? query)
    //{
    //    if (query is null) return null;

    //    var posts = await query.ToListAsync();
    //    return posts.AsQueryable();
    //}

    public override ValueTask<IQueryable<object>?> GetViews(BaseAppUser? requestingUser, IQueryable<Post>? query)
    {
        var requester = (SocialAppUser?)requestingUser;
        return ValueTask.FromResult<IQueryable<object>?>((
                query is null
                ? null
                : requester is null
                ? query.AsNoTracking().Include(x => x.Poster).Where(x => x.Poster != null && x.Poster.Settings.HasFlag(UserSettings.AllowAnonymousPostViews))
                : query.AsNoTracking().Include(x => x.Poster).Where(x => x.Poster != null && (x.Poster.Id == requester.Id || x.Poster.Settings.HasFlag(UserSettings.AllowNonFollowerPostViews) || x.Poster!.Followers != null && x.Poster!.Followers.Contains(requester)))
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
    }

    public override async ValueTask<SuccessResult<object>> GetView(BaseAppUser? r, Post entity)
    {
        var requester = (SocialAppUser?)r;
        var errorlist = new ErrorList();

        var poster = await GetPoster(entity);
        if (await CanView(requester, poster))
        {
            return new SuccessResult<object>(new PostViewModel()
            {
                Content = entity.Content,
                DatePosted = entity.DatePosted,
                Id = entity.Id.AsLong(),
                Poster = new UserViewModel()
                {
                    UserId = poster.Id,
                    Username = poster.UserName!,
                    ProfilePictureUrl = poster.ProfilePictureUrl,
                    Pronouns = poster.Pronouns
                },
                InResponseTo = entity.InResponseToId?.AsLong(),
                PosterId = entity.Poster!.Id,
                PosterThenUsername = entity.PosterThenUsername,
                Responses = entity.Responses?.Select(x => entity.Id.AsLong()).ToHashSet()
            });
        }
        else
        {
            errorlist.AddError(ErrorMessages.NoPermission());
            return new SuccessResult<object>(errorlist);
        }
    }

    public ValueTask<IQueryable<Post>> GetPosts(SocialAppUser requester)
        => ValueTask.FromResult(context.Posts.OrderBy(x => x.DatePosted).Where(x => x.PosterId == requester.Id));

    public async ValueTask<IQueryable<Post>?> GetPosts(SocialAppUser? requester, SocialAppUser user)
        => await CanView(requester, user) ? await GetPosts(user) : null;

    public async ValueTask<SocialAppUser> GetPoster(Post post)
        => post.Poster ?? (await userRepository.Find(post.PosterId))!;

    private async ValueTask<bool> CanView(SocialAppUser? requester, SocialAppUser poster)
        => requester?.Id.Equals(poster.Id) is true
        || (poster.Settings.HasFlag(UserSettings.AllowAnonymousViews) || requester is not null)
            && (poster.Settings.HasFlag(UserSettings.AllowNonFollowerViews) || requester is not null && await userRepository.IsFollowing(requester, poster));
    
    public async ValueTask<IQueryable<Post>?> GetLatestPosts(SocialAppUser? requester, int count)
    {
        if (requester?.Id is not Guid uid || count is <= 0)
            return null;

        return context.Posts
            .AsNoTracking()
            .OrderBy(x => x.DatePosted)
            .Join(
                context.SocialAppUserFollows,
                o => o.PosterId,
                i => i.FollowerId,
                (o, i) => new { Post = o, Followed = i.FollowedId }
            )
            .Where(x => x.Post.PosterId == uid || x.Post.PosterId == x.Followed)
            .Select(x => x.Post)
            .Take(count)
            .Distinct();

        return context.Database.SqlQueryRaw<Post>(
            $"""
            select distinct Posts.* 
            from Posts 
            left join SocialAppUserFollows on SocialAppUserFollows.FollowerId = '{uid}'
            where Posts.PosterId = '{uid}' or Posts.PosterId = SocialAppUserFollows.FollowedId
            order by DatePosted desc
            """
        );
    }

    public async ValueTask<bool> AddLike(SocialAppUser requester, Post post)
    {
        Debug.Assert(requester is not null);
        Debug.Assert(post is not null);

        if (await context.Set<SocialAppUserLike>().AnyAsync(x => x.UserWhoLikedThisId == requester.Id && x.PostId == post.Id) is false)
        {
            await context.Set<SocialAppUserLike>().AddAsync(new SocialAppUserLike()
            {
                Post = post,
                UserWhoLikedThis = requester
            });
            return true;
        }

        return false;
    }

    public async ValueTask<bool> RemoveLike(SocialAppUser requester, Post post)
    {
        Debug.Assert(requester is not null);
        Debug.Assert(post is not null);

        var found = await context.Set<SocialAppUserLike>().FirstOrDefaultAsync(x => x.UserWhoLikedThisId == requester.Id && x.PostId == post.Id);
        if (found is not null)
        {
            context.Set<SocialAppUserLike>().Remove(found);
            return true;
        }

        return false;
    }
}
