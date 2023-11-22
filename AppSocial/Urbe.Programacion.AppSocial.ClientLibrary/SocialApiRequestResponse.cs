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

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public readonly struct SocialApiRequestResponse
{
    private struct ApiResponseBuffer<T>
        where T : class
    {
        public string BearerToken { get; set; }
        public SocialAPIResponseCode Code { get; set; }
        public T[]? Data { get; set; }
        public ErrorMessage[]? Errors { get; set; }
        public string? TraceId { get; set; }
        public string? Exception { get; set; }
    }

    public BearerAPIResponse APIResponse { get; }
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

    private static BearerAPIResponse FillResponse<T>(JsonDocument doc, JsonSerializerOptions? options)
        where T : class
    {
        var t = doc.Deserialize<ApiResponseBuffer<T>>(options);
        return new BearerAPIResponse(t.Code)
        {
            BearerToken = t.BearerToken,
            Data = t.Data,
            Errors = t.Errors,
            Exception = t.Exception,
            TraceId = t.TraceId
        };
    }

    internal SocialApiRequestResponse(BearerAPIResponse apiResponse, HttpStatusCode httpStatusCode)
    {
        APIResponse = apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));
        HttpStatusCode = httpStatusCode;
    }
}

