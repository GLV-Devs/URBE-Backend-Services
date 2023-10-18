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
    public static async ValueTask<APIResponse> GetResponse(this IEnumerable<IResponseModel> data)
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
            Data = data
        };
    }

    public static APIResponse GetResponse(this ErrorList errorList)
        => new(APIResponseCodeEnum.ErrorCollection)
        {
            Errors = errorList.Errors
        };

    public static APIResponse GetResponse(this IEnumerable<ErrorMessage> errorList)
        => new(APIResponseCodeEnum.ErrorCollection)
        {
            Errors = errorList
        };

    public static APIResponse GetResponse(this IResponseModel data)
        => new(data.APIResponseCode)
        {
            Data = new object[] { data }
        };
}
