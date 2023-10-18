using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;
using Urbe.BasesDeDatos.AppSocial.ModelServices.Implementations;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;
using Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Requests;
using Urbe.BasesDeDatos.AppSocial.Common;
using System.Diagnostics;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers;

[ApiController]
[Route("/api/identity")]
public sealed class IdentityController : SocialAppController
{
    private readonly UserManager<SocialAppUser> UserManager;
    private readonly SignInManager<SocialAppUser> SignInManager;
    private readonly ILogger<IdentityController> Logger;

    public IdentityController(UserManager<SocialAppUser> userManager, ILogger<IdentityController> logger, SignInManager<SocialAppUser> signInManager)
    {
        UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        SignInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost]
    public async Task<IActionResult> CreateEntity([FromBody] UserCreationModel creationModel, [FromServices] IUserRepository userRepository)
    {
        var result = await userRepository.Create(null, creationModel);

        if (result.TryGetResult(out var created))
        {
            await userRepository.SaveChanges();
            var viewresult = await userRepository.GetView(null, created);
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

        Logger.LogInformation("Attempting to log in as user {user} ({userid})", userLogin.UserNameOrEmail, user.Id.Value);

        var result = await SignInManager.PasswordSignInAsync(user, userLogin.Password, true, false);
        if (result.Succeeded)
        {
            Logger.LogInformation("Succesfully logged in as user {user} ({userid})", userLogin.UserNameOrEmail, user.Id.Value);
            return Ok();
        }
        else if (result.IsLockedOut)
        {
            Logger.LogInformation("Could not log in as user {user} ({userid}), because they're locked out", user.UserName!, user.Id.Value);
            list.AddError(ErrorMessages.LoginLockedOut(user.UserName!));
            return Forbidden(list.Errors);
        }
        else if (result.RequiresTwoFactor)
        {
            Logger.LogInformation("Could not log in as user {user} ({userid}), because they require 2FA", user.UserName!, user.Id.Value);
            list.AddError(ErrorMessages.LoginRequires("2FA", user.UserName!));
            return Forbidden(list.Errors);
        }
        else if (result.IsNotAllowed)
        {
            Logger.LogInformation("Could not log in as user {user} ({userid}), because they're not allowed to", user.UserName!, user.Id.Value);
            list.AddError(ErrorMessages.ActionDisallowed("LogIn"));
            return Forbidden(list.Errors);
        }
        else
        {
            Logger.LogInformation("Could not log in as user {user} ({userid})", user.UserName!, user.Id.Value);
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
}
