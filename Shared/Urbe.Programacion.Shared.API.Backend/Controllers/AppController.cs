using System.Net;
using Microsoft.AspNetCore.Mvc;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.Shared.API.Backend.Controllers;

public abstract class AppController : Controller
{
    protected virtual IActionResult Forbidden(object? value)
        => new ObjectResult(value) { StatusCode = (int)HttpStatusCode.Forbidden };

    protected virtual IActionResult FailureResult(SuccessResult result)
        => new ObjectResult(result.ErrorMessages.Errors) { StatusCode = (int)(result.ErrorMessages.RecommendedCode ?? HttpStatusCode.InternalServerError) };

    protected virtual IActionResult FailureResult<T>(SuccessResult<T> result)
        => new ObjectResult(result.ErrorMessages.Errors) { StatusCode = (int)(result.ErrorMessages.RecommendedCode ?? HttpStatusCode.InternalServerError) };
}
