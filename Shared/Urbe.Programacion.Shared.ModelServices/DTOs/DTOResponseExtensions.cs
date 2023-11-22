using Azure;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.Shared.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Urbe.Programacion.Shared.ModelServices.DTOs;

public static class DTOResponseExtensions
{
    public static async ValueTask<APIResponse<TObjectCode>> GetResponse<TObjectCode>(
        this IEnumerable<IResponseModel<TObjectCode>> data, string? traceId, APIResponse<TObjectCode>? response)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
    {
        TObjectCode code =
            data is IQueryable<IResponseModel<TObjectCode>> queryable
            ? await queryable.AnyAsync() is false
            ? TObjectCode.NoData
            : (await queryable.FirstAsync()).APIResponseCode
            : data.Any() is false
            ? TObjectCode.NoData
            : data.First().APIResponseCode;

        var r = response ?? new(code);
        r.Data = data;
        r.TraceId = traceId;
        return r;
    }

    public static APIResponse<TObjectCode> GetResponse<TObjectCode>(this ErrorList errorList, string? traceId, APIResponse<TObjectCode>? response)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
    {
        var r = response ?? new(TObjectCode.ErrorCollection);
        r.Errors = errorList.Errors;
        r.TraceId = traceId;
        return r;
    }

    public static APIResponse<TObjectCode> GetResponse<TObjectCode>(this IEnumerable<ErrorMessage> errorList, string? traceId, APIResponse<TObjectCode>? response)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
    {
        var r = response ?? new(TObjectCode.ErrorCollection);
        r.Errors = errorList;
        r.TraceId = traceId;
        return r;
    }

    public static APIResponse<TObjectCode> GetResponse<TObjectCode>(this IResponseModel<TObjectCode> data, string? traceId, APIResponse<TObjectCode>? response)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
    {
        var r = response ?? new(data.APIResponseCode);
        r.Data = new object[] { data };
        r.TraceId = traceId;
        return r;
    }
}
