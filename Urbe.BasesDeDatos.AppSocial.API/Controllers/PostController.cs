using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;
using Urbe.BasesDeDatos.AppSocial.ModelServices.Implementations;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;
using Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Requests;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers;

[ApiController]
[Route("/api/post")]
public class PostController : CRDController<Post, Snowflake, PostCreationModel>
{
    protected IPostRepository PostRepository { get; }
    protected IUserRepository UserRepository { get; }

    public PostController(IPostRepository entityRepository, IUserRepository userRepository, UserManager<SocialAppUser> userManager) : base(entityRepository, userManager)
    {
        PostRepository = entityRepository;
        UserRepository = userRepository;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetPosts()
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        return Ok(await PostRepository.GetPosts(u));
    }

    [HttpGet("from/{userid}")]
    public async Task<IActionResult> GetPosts(Guid userid)
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        var foundEntity = await UserRepository.Find(userid);
        return foundEntity is null ? NotFound() : Ok(await PostRepository.GetPosts(u, foundEntity));
    }
}
