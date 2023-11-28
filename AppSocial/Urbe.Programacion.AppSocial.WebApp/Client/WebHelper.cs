using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.ClientLibrary;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Urbe.Programacion.Shared.Common;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;

namespace Urbe.Programacion.AppSocial.WebApp.Client;

public static class WebHelper
{
    public static string RenderPostTime(this DateTimeOffset dto)
        => dto.ToLocalTime().DateTime.ToString("dd/MM/yy hh:mm tt");

    public static string GetUserProfilePic(this UserViewModel? user)
        => user?.ProfilePictureUrl ?? "/defaultuserpic.jpg";

    public static string GetUserProfilePic(this UserSelfViewModel? user)
        => user?.ProfilePictureUrl ?? "/defaultuserpic.jpg";

    public static async ValueTask<bool> VerifyUserState(this AppState state, SocialApiClient client, NavigationManager nav, ILogger log, CancellationToken ct = default)
    {
        log.LogInformation("Verifying User State");
        if (state.LoggedInUser is not null)
        {
            log.LogDebug("User already present");
            return true;
        }

        var token = await state.GetToken();
        if (string.IsNullOrWhiteSpace(token) is false)
        {
            client.Authorization = new AuthenticationHeaderValue("Bearer", token);
            log.LogDebug("Obtained Bearer Token from local storage");
        }

        var resp = await client.Users.GetMe(ct);
        if (resp.HttpStatusCode == HttpStatusCode.OK)
        {
            log.LogDebug("Obtained User info from API");
            Debug.Assert(resp.APIResponse.Code.ResponseId == APIResponseCodeEnum.UserSelfView);
            var data = resp.APIResponse.Data?.Cast<UserSelfViewModel>().FirstOrDefault();
            Debug.Assert(data is not null);
            await state.SetToken(resp.APIResponse.BearerToken);
            state.LoggedInUser = data;
            return true;
        }

        log.LogInformation("Navigating to Login Page");
        nav.NavigateTo("/Login");
        return false;
    }
}
