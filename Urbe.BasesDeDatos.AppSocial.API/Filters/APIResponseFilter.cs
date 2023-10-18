using DiegoG.REST.ASPNET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            if (objresult.Value is APIResponse) return;

            objresult.Value = objresult.Value is IResponseModel model
                ? model.GetResponse()
                : objresult.Value is IEnumerable<IResponseModel> models
                ? (object)await models.GetResponse()
                : throw new InvalidDataException("The result of the request was, unexpectedly, not an APIResponse or an IResponseModel");
            
            await next();
        }

        throw new InvalidDataException("The result of the request was, unexpectedly, not an ObjectResult");
    }
}
