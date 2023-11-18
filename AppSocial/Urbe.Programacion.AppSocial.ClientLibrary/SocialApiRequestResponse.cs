using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Urbe.Programacion.AppSocial.DataTransfer;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public readonly struct SocialApiRequestResponse
{
    private struct ApiResponseBuffer<T>
        where T : class
    {
        public SocialAPIResponseCode Code;
        public T[]? Data;
        public IEnumerable<ErrorMessage>? Errors;
        public string? TraceId;
        public string? Exception;
    }

    public APIResponse<SocialAPIResponseCode> APIResponse { get; }
    public HttpStatusCode HttpStatusCode { get; }

    public static async Task<SocialApiRequestResponse> FromResponse(
        Task<HttpResponseMessage> response, 
        JsonSerializerOptions? jsonOptions, 
        CancellationToken ct
    )
    {
        var resp = await response;
        using var stream = await resp.Content.ReadAsStreamAsync(ct);
        var jdoc = JsonDocument.Parse(stream);
        var code = (APIResponseCodeEnum)jdoc.RootElement.GetProperty("code").GetProperty("responseId").GetInt32();

        return new SocialApiRequestResponse(
            code switch
            {
                APIResponseCodeEnum.PostView => FillResponse<PostViewModel>(jdoc, jsonOptions),
                APIResponseCodeEnum.UserSelfView => FillResponse<UserSelfViewModel>(jdoc, jsonOptions),
                APIResponseCodeEnum.UserView => FillResponse<UserViewModel>(jdoc, jsonOptions),
                APIResponseCodeEnum.NoData => FillResponse<object>(jdoc, jsonOptions),
                APIResponseCodeEnum.Success => FillResponse<object>(jdoc, jsonOptions),
                APIResponseCodeEnum.Empty => FillResponse<object>(jdoc, jsonOptions),
                APIResponseCodeEnum.ErrorCollection => FillResponse<object>(jdoc, jsonOptions),
                APIResponseCodeEnum.Exception => FillResponse<object>(jdoc, jsonOptions),
                APIResponseCodeEnum.UnspecifiedError => FillResponse<object>(jdoc, jsonOptions),
                _ => throw new NotImplementedException(),
            },
            resp.StatusCode
        );
    }

    private static APIResponse<SocialAPIResponseCode> FillResponse<T>(JsonDocument doc, JsonSerializerOptions? options)
        where T : class
    {
        var t = doc.Deserialize<ApiResponseBuffer<T>>(options);
        return new APIResponse<SocialAPIResponseCode>(t.Code)
        {
            Data = t.Data,
            Errors = t.Errors,
            Exception = t.Exception,
            TraceId = t.TraceId
        };
    }

    internal SocialApiRequestResponse(APIResponse<SocialAPIResponseCode> apiResponse, HttpStatusCode httpStatusCode)
    {
        APIResponse = apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));
        HttpStatusCode = httpStatusCode;
    }
}

