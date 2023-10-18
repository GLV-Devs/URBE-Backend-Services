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
#if DEBUG
        if (data is IQueryable<IResponseModel> queryable)
            data = await queryable.ToListAsync();

        APIResponseCode code = data.First().APIResponseCode;

        foreach (var d in data)
            if (code != d.APIResponseCode) throw new InvalidOperationException("Code mismatch within collection");
#else
        APIResponseCode code = data is IQueryable<IResponseModel> queryable ? (await queryable.FirstAsync()).APIResponseCode : data.First().APIResponseCode;
#endif

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
