using System.Net;
using Microsoft.AspNetCore.Mvc;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.API.Controllers.Base;

public abstract class SocialAppController : Controller
{
    protected IActionResult Forbidden(object? value)
        => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.Forbidden };

    protected IActionResult FailureResult(SuccessResult result)
        => new ObjectResult(result.ErrorMessages.Errors) { StatusCode = (int)(result.ErrorMessages.RecommendedCode ?? HttpStatusCode.InternalServerError) };

    protected IActionResult FailureResult<T>(SuccessResult<T> result)
        => new ObjectResult(result.ErrorMessages.Errors) { StatusCode = (int)(result.ErrorMessages.RecommendedCode ?? HttpStatusCode.InternalServerError) };
}
