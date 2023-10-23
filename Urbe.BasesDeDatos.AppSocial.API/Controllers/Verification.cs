﻿using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers;

[ApiController]
[Route("api/verification")]
public class Verification : SocialAppController
{
    public IUserVerificationService UserVerificationService { get; }

    public Verification(IUserVerificationService userVerificationService)
    {
        UserVerificationService = userVerificationService ?? throw new ArgumentNullException(nameof(userVerificationService));
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> RequestVerification([FromServices] UserManager<SocialAppUser> userManager)
    {
        var u = await userManager.GetUserAsync(User);
        Debug.Assert(u is not null);

        var errorlist = new ErrorList();
        if (await UserVerificationService.CreateMailConfirmation(u))
            return Ok();
        else
        {
            if (await userManager.IsEmailConfirmedAsync(u))
                errorlist.AddError(ErrorMessages.EmailAlreadyConfirmed());
            else
                errorlist.AddError(ErrorMessages.VerificationRequestAlreadyActive());
            return BadRequest(errorlist.Errors);
        }
    }

    [HttpGet("{token}")]
    public async Task<IActionResult> Verify(string token)
    {
        if (RandomKey.TryParse(token, null, out var key) is false)
            return NotFound();

        var confirmation = await UserVerificationService.FindById(key);
        if (confirmation is null)
            return NotFound();

        var result = await UserVerificationService.VerifyUserEmail(confirmation);
        return result switch
        {
            VerificationResult.Approved => Ok(),
            VerificationResult.Rejected => Forbidden(null),
            VerificationResult.Expired => BadRequest(),
            _ => throw new NotSupportedException($"Unknown VerificationResult: {result}")
        };
    }
}
