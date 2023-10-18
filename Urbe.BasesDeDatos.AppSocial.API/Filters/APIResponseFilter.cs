using System.Diagnostics;
using System.Net;
using DiegoG.REST.ASPNET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;
using Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs;

namespace Urbe.BasesDeDatos.AppSocial.API.Filters;

public class FilterAPIResponseAttribute : ResultFilterAttribute
{
    public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        => APIResponseFilter.Instance.OnResultExecutionAsync(context, next);
}

public sealed class APIResponseFilter : IAsyncResultFilter
{
    private APIResponseFilter() { }

    public static APIResponseFilter Instance { get; } = new();

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objresult)
        {
            switch (objresult.Value)
            {
                case APIResponse:
                    break;

                case null:
                    objresult.Value = objresult.StatusCode is int sc && sc >= 200 && sc <= 299
                        ? new APIResponse(APIResponseCodeEnum.Success) { TraceId = context.HttpContext.TraceIdentifier }
                        : new APIResponse(APIResponseCodeEnum.UnspecifiedError) { TraceId = context.HttpContext.TraceIdentifier };
                    break;

                case ProblemDetails problem:
                    var pdlist = new ErrorList();
                    pdlist.AddError(new ErrorMessage($"{problem.Title}: {problem.Detail}", "Unknown", null));
                    objresult.Value = new APIResponse(APIResponseCodeEnum.ErrorCollection)
                    {
                        Errors = pdlist,
                        TraceId = context.HttpContext.TraceIdentifier
                    };
                    break;

                case IResponseModel model:
                    objresult.Value = model.GetResponse(context.HttpContext.TraceIdentifier);
                    break;

                case IEnumerable<IResponseModel> models:
                    objresult.Value = await models.GetResponse(context.HttpContext.TraceIdentifier);
                    break;

                case ErrorList errorList:
                    objresult.Value = errorList.GetResponse(context.HttpContext.TraceIdentifier);
                    break;

                case IEnumerable<ErrorMessage> errors:
                    objresult.Value = errors.GetResponse(context.HttpContext.TraceIdentifier);
                    break;

                default:
                    Debugger.Break();
                    throw new InvalidDataException("The result of the request was, unexpectedly, not an APIResponse or an IResponseModel");
            }

            await next();
        }
        else if (context.Result is StatusCodeResult statusResult)
            context.Result = new ObjectResult(statusResult.StatusCode is int sc && sc >= 200 && sc <= 299
                        ? new APIResponse(APIResponseCodeEnum.Success) { TraceId = context.HttpContext.TraceIdentifier }
                        : new APIResponse(APIResponseCodeEnum.UnspecifiedError) { TraceId = context.HttpContext.TraceIdentifier });
        else if (context.Result is null)
            context.Result = new ObjectResult(new APIResponse(APIResponseCodeEnum.Success) { TraceId = context.HttpContext.TraceIdentifier });
        else
        {
            Debugger.Break();
            throw new InvalidDataException("The result of the request was, unexpectedly, not an ObjectResult");
        }
    }
}
