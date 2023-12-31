﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.AppSocial.ModelServices;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.Shared.API.Backend.Controllers;
using Urbe.Programacion.Shared.Common;
using Microsoft.AspNetCore.Cors;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Controllers;

[ApiController]
[Route("/api/identity")]
public sealed class IdentityController : AppController
{
    private readonly UserManager<SocialAppUser> UserManager;
    private readonly SignInManager<SocialAppUser> SignInManager;
    private readonly ILogger<IdentityController> Logger;
    private readonly IUserRepository UserRepository;

    public IdentityController(IUserRepository userRepository, UserManager<SocialAppUser> userManager, ILogger<IdentityController> logger, SignInManager<SocialAppUser> signInManager)
    {
        UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet("contest")]
    public IActionResult Contest()
        => BadRequest();

    [HttpPost]
    public async Task<IActionResult> CreateEntity([FromBody] UserCreationModel creationModel)
    {
        var result = await UserRepository.Create(null, creationModel);

        if (result.TryGetResult(out var created))
        {
            await UserRepository.SaveChanges();
            var viewresult = await UserRepository.GetSelfView(created);
            return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
        }

        return FailureResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> Login([FromBody] UserLoginModel? userLogin)
    {
        ErrorList list = new();

        if (userLogin is null)
        {
            list.AddError(ErrorMessages.EmptyBody());
            return BadRequest(list.Errors);
        }

        if (string.IsNullOrWhiteSpace(userLogin.UserNameOrEmail))
            list.AddError(ErrorMessages.BadUsername(userLogin.UserNameOrEmail ?? ""));

        if (string.IsNullOrWhiteSpace(userLogin.Password))
            list.AddError(ErrorMessages.BadPassword());

        if (list.Count > 0)
            return BadRequest(list.Errors);

        Debug.Assert(string.IsNullOrWhiteSpace(userLogin.UserNameOrEmail) is false);
        Debug.Assert(string.IsNullOrWhiteSpace(userLogin.Password) is false);
        var user = await UserManager.FindByEmailAsync(userLogin.UserNameOrEmail) ?? await UserManager.FindByNameAsync(userLogin.UserNameOrEmail);
        if (user is null)
        {
            list.AddError(ErrorMessages.UserNotFound(userLogin.UserNameOrEmail));
            return NotFound(list.Errors);
        }

        Logger.LogInformation("Attempting to log in as user {user} ({userid})", userLogin.UserNameOrEmail, user.Id);

        var result = await SignInManager.PasswordSignInAsync(user, userLogin.Password, true, false);
        if (result.Succeeded)
        {
            Logger.LogInformation("Succesfully logged in as user {user} ({userid})", userLogin.UserNameOrEmail, user.Id);
            var viewresult = await UserRepository.GetSelfView(user);
            return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
        }
        else if (result.IsLockedOut)
        {
            Logger.LogInformation("Could not log in as user {user} ({userid}), because they're locked out", user.UserName!, user.Id);
            list.AddError(ErrorMessages.LoginLockedOut(user.UserName!));
            return Forbidden(list.Errors);
        }
        else if (result.RequiresTwoFactor)
        {
            Logger.LogInformation("Could not log in as user {user} ({userid}), because they require 2FA", user.UserName!, user.Id);
            list.AddError(ErrorMessages.LoginRequires("2FA", user.UserName!));
            return Forbidden(list.Errors);
        }
        else if (result.IsNotAllowed)
        {
            Logger.LogInformation("Could not log in as user {user} ({userid}), because they're not allowed to", user.UserName!, user.Id);
            list.AddError(ErrorMessages.ActionDisallowed("LogIn"));
            return Forbidden(list.Errors);
        }
        else
        {
            Logger.LogInformation("Could not log in as user {user} ({userid})", user.UserName!, user.Id);
            list.AddError(ErrorMessages.BadLogin());
            return BadRequest(list.Errors);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Logout()
    {
        await SignInManager.SignOutAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var u = await UserManager.GetUserAsync(User);
        if (u is null || string.Equals(u.Email, "admin@admin.com", StringComparison.OrdinalIgnoreCase) is false)
            return Unauthorized();

        var entity = await UserRepository.Find(userId);
        if (entity is null)
            return NotFound();

        var result = await UserRepository.Delete(u, entity);

        return result.IsSuccess ? Ok() : FailureResult(result);
    }
}
