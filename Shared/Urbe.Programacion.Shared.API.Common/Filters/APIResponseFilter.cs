using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.ModelServices.DTOs;

namespace Urbe.Programacion.Shared.API.Common.Filters;

public sealed class APIResponseFilter<TObjectCode> : IAsyncResultFilter
    where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
{
    private APIResponseFilter() { }

    public static APIResponseFilter<TObjectCode> Instance { get; } = new();

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objresult)
        {
            switch (objresult.Value)
            {
                case APIResponse<TObjectCode>:
                    break;

                case null:
                    objresult.Value = objresult.StatusCode is int sc && sc >= 200 && sc <= 299
                        ? new APIResponse<TObjectCode>(TObjectCode.Success) { TraceId = context.HttpContext.TraceIdentifier }
                        : new APIResponse<TObjectCode>(TObjectCode.UnspecifiedError) { TraceId = context.HttpContext.TraceIdentifier };
                    break;

                case ProblemDetails problem:
                    var pdlist = new ErrorList();
                    pdlist.AddError(new ErrorMessage($"{problem.Title}: {problem.Detail}", "Unknown", null));
                    objresult.Value = new APIResponse<TObjectCode>(TObjectCode.ErrorCollection)
                    {
                        Errors = pdlist.Errors,
                        TraceId = context.HttpContext.TraceIdentifier
                    };
                    break;

                case IResponseModel<TObjectCode> model:
                    objresult.Value = model.GetResponse(context.HttpContext.TraceIdentifier);
                    break;

                case IEnumerable<IResponseModel<TObjectCode>> models:
                    objresult.Value = await models.GetResponse(context.HttpContext.TraceIdentifier);
                    break;

                case ErrorList errorList:
                    objresult.Value = errorList.GetResponse<TObjectCode>(context.HttpContext.TraceIdentifier);
                    break;

                case IEnumerable<ErrorMessage> errors:
                    objresult.Value = errors.GetResponse<TObjectCode>(context.HttpContext.TraceIdentifier);
                    break;

                default:
                    Debugger.Break();
                    throw new InvalidDataException("The result of the request was, unexpectedly, not an APIResponse or an IResponseModel");
            }

            await next();
        }
        else if (context.Result is StatusCodeResult statusResult)
            context.Result = new ObjectResult(statusResult.StatusCode is int sc && sc >= 200 && sc <= 299
                        ? new APIResponse<TObjectCode>(TObjectCode.Success) { TraceId = context.HttpContext.TraceIdentifier }
                        : new APIResponse<TObjectCode>(TObjectCode.UnspecifiedError) { TraceId = context.HttpContext.TraceIdentifier });
        else if (context.Result is null)
            context.Result = new ObjectResult(new APIResponse<TObjectCode>(TObjectCode.Success) { TraceId = context.HttpContext.TraceIdentifier });
        else
        {
            Debugger.Break();
            throw new InvalidDataException("The result of the request was, unexpectedly, not an ObjectResult");
        }
    }
}
