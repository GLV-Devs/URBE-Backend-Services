using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;
using Urbe.BasesDeDatos.AppSocial.ModelServices.Implementations;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;
using Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Requests;

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
    public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin) 
    {
        if (string.IsNullOrWhiteSpace(userLogin.UserNameOrEmail))
            return BadRequest();

        if (string.IsNullOrWhiteSpace(userLogin.Password))
            return BadRequest();

        var user = await UserManager.FindByEmailAsync(userLogin.UserNameOrEmail) ?? await UserManager.FindByNameAsync(userLogin.UserNameOrEmail);
        if (user is null)
            return NotFound();

        Logger.LogInformation("Attempting to log in as user {user} ({userid})", userLogin.UserNameOrEmail);

        var result = await SignInManager.PasswordSignInAsync(user, userLogin.Password, true, false);
        if (result.Succeeded)
        {
            Logger.LogInformation("Succesfully logged in as user {user} ({userid})", userLogin.UserNameOrEmail, user.Id.Value);
            return Ok();
        }
        else
        {
            Logger.LogInformation("Could not log in as user {user} ({userid})", userLogin.UserNameOrEmail, user.Id.Value);
            return BadRequest();
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
