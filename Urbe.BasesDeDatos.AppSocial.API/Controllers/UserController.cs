using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers;

[ApiController]
[Route("/api/user")]
public class UserController : SocialAppController
{
    protected IUserRepository UserRepository { get; }
    protected UserManager<SocialAppUser> UserManager { get; }

    protected UserController(IUserRepository userRepository, UserManager<SocialAppUser> userManager)
    {
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
    }

    [HttpGet("query")]
    [EnableQuery]
    public async Task<ActionResult<IQueryable<UserViewModel>>> QueryUsers()
    {
        var u = await UserManager.GetUserAsync(User);
        return Ok(UserRepository.Query(u));
    }

    [HttpPut("addfollow/{key}")]
    [Authorize]
    public async Task<IActionResult> FollowUser(Guid key)
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);

        var foundEntity = await UserRepository.Find(u, key);
        return foundEntity is null ? NotFound() : await UserRepository.FollowUser(u, foundEntity) ? Ok() : BadRequest();
    }

    #region Regarding Requester and other user

    [HttpGet("mutuals/{key}")]
    [EnableQuery]
    public async Task<IActionResult> GetMutuals(Guid key)
    {
        var u = await UserManager.GetUserAsync(User);

        var foundEntity = await UserRepository.Find(u, key);
        return foundEntity is null ? NotFound() : Ok(await UserRepository.GetViews(u, await UserRepository.GetMutuals(u, foundEntity)));
    }

    [HttpGet("followed/{key}")]
    [EnableQuery]
    public async Task<IActionResult> GetFollowed(Guid key)
    {
        var u = await UserManager.GetUserAsync(User);

        var foundEntity = await UserRepository.Find(u, key);
        return foundEntity is null ? NotFound() : Ok(await UserRepository.GetViews(u, await UserRepository.GetFollowing(u, foundEntity)));
    }

    [HttpGet("followers/{key}")]
    [EnableQuery]
    public async Task<IActionResult> GetFollowers(Guid key)
    {
        var u = await UserManager.GetUserAsync(User);
        
        var foundEntity = await UserRepository.Find(u, key);
        return foundEntity is null ? NotFound() : Ok(await UserRepository.GetViews(u, await UserRepository.GetFollowers(u, foundEntity)));
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> ViewEntity(Guid key)
    {
        var u = await UserManager.GetUserAsync(User);
        var foundEntity = await UserRepository.Find(u, key);
        if (foundEntity is null)
            return NotFound();

        var viewresult = await UserRepository.GetView(u, foundEntity);
        return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
    }

    #endregion

    #region Regarding Requester

    [HttpGet("mutuals")]
    [EnableQuery]
    [Authorize]
    public async Task<IActionResult> GetMutuals()
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        return Ok(await UserRepository.GetViews(u, await UserRepository.GetMutuals(u)));
    }

    [HttpGet("followed")]
    [EnableQuery]
    [Authorize]
    public async Task<IActionResult> GetFollowed()
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        return Ok(await UserRepository.GetViews(u, await UserRepository.GetFollowing(u)));
    }

    [HttpGet("followers")]
    [EnableQuery]
    [Authorize]
    public async Task<IActionResult> GetFollowers()
    {
        var u = await UserManager.GetUserAsync(User);
        Debug.Assert(u is not null);
        return Ok(await UserRepository.GetViews(u, await UserRepository.GetFollowers(u)));
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] UserUpdateModel update)
    {
        var u = await UserManager.GetUserAsync(User);
        if (u is null)
            return NotFound();

        var result = await UserRepository.Update(u, u, update);

        if (result.IsSuccess)
        {
            await UserRepository.SaveChanges();
            var viewresult = await UserRepository.GetView(u, u);

            return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
        }

        return FailureResult(result);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ViewEntity()
    {
        var u = await UserManager.GetUserAsync(User);
        if (u is null)
            return NotFound();

        var viewresult = await UserRepository.GetView(u, u);
        return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
    }

    #endregion
}
