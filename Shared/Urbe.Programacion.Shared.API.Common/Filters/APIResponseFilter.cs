using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Urbe.Programacion.Shared.Common;
using Urbe.Programacion.Shared.ModelServices.DTOs;

namespace Urbe.Programacion.Shared.API.Common.Filters;

public class APIResponseFilter<TObjectCode> : IAsyncResultFilter
    where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
{
    protected APIResponseFilter() { }

    public static APIResponseFilter<TObjectCode> Instance { get; } = new();

    protected virtual APIResponse<TObjectCode> CreateAPIResponseObject(TObjectCode code, ResultExecutingContext context)
        => new(code);

    protected async ValueTask<APIResponse<TObjectCode>> FillAPIResponseObject(ResultExecutingContext context)
    {
        APIResponse<TObjectCode> resp;
        if (context.Result is ObjectResult objresult)
        {
            switch (objresult.Value)
            {
                case APIResponse<TObjectCode>:

                case null:
                    resp = objresult.StatusCode is int sc && sc >= 200 && sc <= 299
                        ? CreateAPIResponseObject(TObjectCode.Success, context)
                        : CreateAPIResponseObject(TObjectCode.UnspecifiedError, context);

                    resp.TraceId = context.HttpContext.TraceIdentifier;
                    return resp;

                case ProblemDetails problem:
                    var pdlist = new ErrorList();
                    pdlist.AddError(new ErrorMessage($"{problem.Title}: {problem.Detail}", "Unknown", null));
                    resp = CreateAPIResponseObject(TObjectCode.ErrorCollection, context);
                    resp.Errors = pdlist.Errors;
                    resp.TraceId = context.HttpContext.TraceIdentifier;
                    return resp;

                case IResponseModel<TObjectCode> model:
                    return model.GetResponse(context.HttpContext.TraceIdentifier, CreateAPIResponseObject(TObjectCode.ErrorCollection, context));

                case IEnumerable<IResponseModel<TObjectCode>> models:
                    return await models.GetResponse(context.HttpContext.TraceIdentifier, CreateAPIResponseObject(TObjectCode.ErrorCollection, context));

                case ErrorList errorList:
                    return errorList.GetResponse(context.HttpContext.TraceIdentifier, CreateAPIResponseObject(TObjectCode.ErrorCollection, context));

                case IEnumerable<ErrorMessage> errors:
                    return errors.GetResponse(context.HttpContext.TraceIdentifier, CreateAPIResponseObject(TObjectCode.ErrorCollection, context));

                default:
                    Debugger.Break();
                    throw new InvalidDataException("The result of the request was, unexpectedly, not an APIResponse or an IResponseModel");
            }
        }
        else if (context.Result is StatusCodeResult statusResult)
        {
            resp = statusResult.StatusCode is int sc && sc >= 200 && sc <= 299
                ? CreateAPIResponseObject(TObjectCode.Success, context)
                : CreateAPIResponseObject(TObjectCode.UnspecifiedError, context);

            resp.TraceId = context.HttpContext.TraceIdentifier;
            return resp;
        }
        else if (context.Result is null)
        {
            resp = CreateAPIResponseObject(TObjectCode.Success, context);
            resp.TraceId = context.HttpContext.TraceIdentifier;
            return resp;
        }
        else
        {
            Debugger.Break();
            throw new InvalidDataException("The result of the request was, unexpectedly, not an ObjectResult");
        }
    }

    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        await next.Invoke();
        context.Result = new ObjectResult(await FillAPIResponseObject(context));
    }
}
