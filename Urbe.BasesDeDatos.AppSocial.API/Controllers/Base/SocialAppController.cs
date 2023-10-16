using Microsoft.AspNetCore.Mvc;
using Urbe.BasesDeDatos.AppSocial.Common;

namespace Urbe.BasesDeDatos.AppSocial.API.Controllers.Base;

public abstract class SocialAppController : Controller
{
    protected IActionResult FailureResult(SuccessResult result)
        => new ObjectResult(result.ErrorMessages) { StatusCode = (int)(result.ErrorMessages.RecommendedCode ?? System.Net.HttpStatusCode.InternalServerError) };

    protected IActionResult FailureResult<T>(SuccessResult<T> result)
        => new ObjectResult(result.ErrorMessages) { StatusCode = (int)(result.ErrorMessages.RecommendedCode ?? System.Net.HttpStatusCode.InternalServerError) };
}
