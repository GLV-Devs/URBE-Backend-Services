using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.Shared.ModelServices.DTOs;

public static class DTOResponseExtensions
{
    public static async ValueTask<APIResponse<TObjectCode>> GetResponse<TObjectCode>(this IEnumerable<IResponseModel<TObjectCode>> data, string? traceId)
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

        return new(code)
        {
            Data = data,
            TraceId = traceId
        };
    }

    public static APIResponse<TObjectCode> GetResponse<TObjectCode>(this ErrorList errorList, string? traceId)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
        => new(TObjectCode.ErrorCollection)
        {
            Errors = errorList.Errors,
            TraceId = traceId
        };

    public static APIResponse<TObjectCode> GetResponse<TObjectCode>(this IEnumerable<ErrorMessage> errorList, string? traceId)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
        => new(TObjectCode.ErrorCollection)
        {
            Errors = errorList,
            TraceId = traceId
        };

    public static APIResponse<TObjectCode> GetResponse<TObjectCode>(this IResponseModel<TObjectCode> data, string? traceId)
        where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
        => new(data.APIResponseCode)
        {
            Data = new object[] { data },
            TraceId = traceId
        };
}
