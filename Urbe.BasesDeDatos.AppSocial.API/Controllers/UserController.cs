using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;
using Urbe.BasesDeDatos.AppSocial.DatabaseServices.Implementations;
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

    [HttpPut]
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
    public virtual async Task<IActionResult> ViewEntity()
    {
        var u = await UserManager.GetUserAsync(User);
        if (u is null)
            return NotFound();

        var viewresult = await UserRepository.GetView(u, u);
        return viewresult.TryGetResult(out var view) ? Ok(view) : FailureResult(viewresult);
    }
}
