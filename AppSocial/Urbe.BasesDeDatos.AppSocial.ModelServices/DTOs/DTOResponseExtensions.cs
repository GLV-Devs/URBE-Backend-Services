using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs;

public static class DTOResponseExtensions
{
    public static async ValueTask<APIResponse> GetResponse(this IEnumerable<IResponseModel> data, string? traceId)
    {
        APIResponseCode code = 
            data is IQueryable<IResponseModel> queryable
            ? await queryable.AnyAsync() is false 
            ? (APIResponseCode)APIResponseCodeEnum.NoData 
            : (await queryable.FirstAsync()).APIResponseCode
            : data.Any() is false 
            ? (APIResponseCode)APIResponseCodeEnum.NoData 
            : (data.First()).APIResponseCode;

        return new(code)
        {
            Data = data,
            TraceId = traceId
        };
    }

    public static APIResponse GetResponse(this ErrorList errorList, string? traceId)
        => new(APIResponseCodeEnum.ErrorCollection)
        {
            Errors = errorList.Errors,
            TraceId = traceId
        };

    public static APIResponse GetResponse(this IEnumerable<ErrorMessage> errorList, string? traceId)
        => new(APIResponseCodeEnum.ErrorCollection)
        {
            Errors = errorList,
            TraceId = traceId
        };

    public static APIResponse GetResponse(this IResponseModel data, string? traceId)
        => new(data.APIResponseCode)
        {
            Data = new object[] { data },
            TraceId = traceId
        };
}
