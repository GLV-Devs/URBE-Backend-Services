using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Urbe.Programacion.AppSocial.DataTransfer;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.ClientLibrary;

public readonly struct SocialApiRequestResponse
{
    private struct ApiResponseBuffer<T>
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
        return new SocialApiRequestResponse(
            (await resp.Content.ReadFromJsonAsync<APIResponse<SocialAPIResponseCode>>(jsonOptions, ct))!,
            resp.StatusCode
        );
    }

    public static async Task<SocialApiRequestResponse> FromResponse<T>(
        Task<HttpResponseMessage> response, 
        JsonSerializerOptions? jsonOptions, 
        CancellationToken ct
    )
    {
        var resp = await response;
        ApiResponseBuffer<T> dat = await resp.Content.ReadFromJsonAsync<ApiResponseBuffer<T>>(jsonOptions, ct);
        return new SocialApiRequestResponse(
            new APIResponse<SocialAPIResponseCode>(dat.Code)
            {
                Data = dat.Data,
                Errors = dat.Errors,
                Exception = dat.Exception,
                TraceId = dat.TraceId
            },
            resp.StatusCode
        );
    }

    internal SocialApiRequestResponse(APIResponse<SocialAPIResponseCode> apiResponse, HttpStatusCode httpStatusCode)
    {
        APIResponse = apiResponse ?? throw new ArgumentNullException(nameof(apiResponse));
        HttpStatusCode = httpStatusCode;
    }
}

