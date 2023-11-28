using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.Shared.API.Backend.Controllers;
using Urbe.Programacion.Shared.Common;
using Microsoft.AspNetCore.Cors;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Controllers;

[ApiController]
[Route("/api/post")]
public class PostController : CRDController<SocialAppUser, Post, Snowflake, PostCreationModel>
{
    protected IPostRepository PostRepository { get; }
    protected IUserRepository UserRepository { get; }

    public PostController(IPostRepository entityRepository, IUserRepository userRepository, UserManager<SocialAppUser> userManager) : base(entityRepository, userManager)
    {
        PostRepository = entityRepository;
        UserRepository = userRepository;
    }

    [HttpGet("responses/{key}")]
    [EnableQuery]
    public async Task<IActionResult> GetResponses(long id)
    {
        var u = await UserManager.GetUserAsync(User);
        var snowflake = new Snowflake(id);
        return Ok(await PostRepository.GetViews(u, PostRepository.Query().Where(x => x.InResponseToId != null && x.InResponseToId == snowflake)));
    }

    [HttpGet("feed")]
    [Authorize]
    public Task<IActionResult> GetLatestPosts()
        => GetLatestPosts(null);

    [HttpPut("like/{id}")]
    [Authorize]
    public async Task<IActionResult> AddLike(long id)
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        var snowflake = new Snowflake(id);
        var found = await PostRepository.Find(u, snowflake);

        if (found is null)
            return NotFound();

        if (await PostRepository.AddLike(u, found))
            await PostRepository.SaveChanges();

        return Ok();
    }

    [HttpPut("unlike/{id}")]
    [Authorize]
    public async Task<IActionResult> RemoveLike(long id)
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        var snowflake = new Snowflake(id);
        var found = await PostRepository.Find(u, snowflake);

        if (found is null)
            return NotFound();

        if (await PostRepository.RemoveLike(u, found))
            await PostRepository.SaveChanges();

        return Ok();
    }

    [HttpGet("feed/{count}")]
    [Authorize]
    public async Task<IActionResult> GetLatestPosts(int? count = null)
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        return Ok(await PostRepository.GetViews(u, await PostRepository.GetLatestPosts(u, count ?? 10)));
    }

    [HttpGet("me")]
    [Authorize]
    [EnableQuery]
    public async Task<IActionResult> GetPosts()
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        return Ok(await PostRepository.GetViews(u, await PostRepository.GetPosts(u)));
    }

    [HttpGet("from/{userid}")]
    [EnableQuery]
    public async Task<IActionResult> GetPosts(Guid userid)
    {
        var u = await UserManager.GetUserAsync(User);
        var foundEntity = await UserRepository.Find(userid);
        return foundEntity is null ? NotFound() : Ok(await PostRepository.GetViews(u, await PostRepository.GetPosts(u, foundEntity)));
    }
}
