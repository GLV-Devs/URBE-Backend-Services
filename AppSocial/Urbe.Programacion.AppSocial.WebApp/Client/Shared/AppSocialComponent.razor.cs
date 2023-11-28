using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.ClientLibrary;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;
using System;
using Majorsoft.Blazor.Extensions.BrowserStorage;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Shared;

public partial class AppSocialComponent
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject] protected SocialApiClient Client { get; set; }
    [Inject] protected AppState AppState { get; set; }
    [Inject] protected NavigationManager Navigation { get; set; }
    [Inject] protected ILocalStorageService LocalStorage { get; set; } 
#pragma warning restore CS8618

    protected ErrorList Errors;

    protected bool CheckResponse(SocialApiRequestResponse response, APIResponseCodeEnum expectedApiResponseCode, HttpStatusCode expectedCode = HttpStatusCode.OK)
    {
        if (Helper.IsExpectedCode(ref Errors, response.HttpStatusCode) is false)
        {
            if (response.APIResponse?.Errors is not null)
                Errors.AddErrorRange(response.APIResponse.Errors);

            StateHasChanged();
            return false;
        }

        if (response.APIResponse.Code.IsExpectedResponse(ref Errors, expectedApiResponseCode) is false)
        {
            if (response.APIResponse.Errors is not null)
                Errors.AddErrorRange(response.APIResponse.Errors);

            StateHasChanged();
            return false;
        }

        return true;
    }

    protected ValueTask VerifyUserState(CancellationToken ct = default)
        => WebHelper.VerifyUserState(AppState, Client, Navigation, ct);
}
