using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DiegoG.REST.ASPNET;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.ModelServices;

namespace Urbe.Programacion.Shared.API.Backend.Services;

public static class AppRESTExtensions
{
    public static IServiceCollection UseAPIResponseInvalidModelStateResponse<TApiResponseCode>(this IServiceCollection services)
        where TApiResponseCode : struct, IEquatable<TApiResponseCode>, IAPIResponseObjectCode<TApiResponseCode>
    {
        return services.UseRESTInvalidModelStateResponse(
            x => new RESTObjectResult<TApiResponseCode>(new APIResponse<TApiResponseCode>(TApiResponseCode.ErrorCollection)
            {
                Data = null,
                Errors = null
            })
        );
    }

    public static IApplicationBuilder UseVerboseExceptionHandler<TApiResponseCode>(this IApplicationBuilder app)
        where TApiResponseCode : struct, IEquatable<TApiResponseCode>, IAPIResponseObjectCode<TApiResponseCode>
    {
        return app.UseRESTExceptionHandler((r, e, s, c) => Task.FromResult(new ExceptionRESTResponse<TApiResponseCode>(
                new APIResponse<TApiResponseCode>(TApiResponseCode.Exception)
                {
                    Exception = e?.ToString() ?? "Unknown error"
                },
                HttpStatusCode.InternalServerError
            )));
    }

    public static IApplicationBuilder UseObfuscatedExceptionHandler<TApiResponseCode>(this IApplicationBuilder app)
        where TApiResponseCode : struct, IEquatable<TApiResponseCode>, IAPIResponseObjectCode<TApiResponseCode>
    {
        return app.UseRESTExceptionHandler((r, e, s, c) =>
        {
            var errors = new ErrorList();
            errors.AddError(ErrorMessages.InternalError());
            return Task.FromResult(new ExceptionRESTResponse<TApiResponseCode>(
                new APIResponse<TApiResponseCode>(TApiResponseCode.ErrorCollection)
                {
                    Errors = errors.Errors
                },
                HttpStatusCode.InternalServerError
            ));
        });
    }
}
