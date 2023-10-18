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
                    return;

                case IResponseModel model:
                    objresult.Value = model.GetResponse();
                    break;

                case IEnumerable<IResponseModel> models:
                    objresult.Value = await models.GetResponse();
                    break;

                case ErrorList errorList:
                    objresult.Value = errorList.GetResponse();
                    break;

                case IEnumerable<ErrorMessage> errors:
                    objresult.Value = errors.GetResponse();
                    break;

                default:
                    throw new InvalidDataException("The result of the request was, unexpectedly, not an APIResponse or an IResponseModel");
            }

            await next();
        }

        throw new InvalidDataException("The result of the request was, unexpectedly, not an ObjectResult");
    }
}
