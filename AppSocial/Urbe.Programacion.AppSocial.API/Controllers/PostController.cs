﻿using System.Diagnostics;
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

namespace Urbe.Programacion.AppSocial.API.Controllers;

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

    [HttpGet]
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
