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
    public static async ValueTask VerifyUserState(this AppState state, SocialApiClient client, NavigationManager nav, CancellationToken ct = default)
    {
        if (state.LoggedInUser is not null)
            return;

        var token = await state.GetToken();
        if (string.IsNullOrWhiteSpace(token) is false)
            client.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var resp = await client.Users.GetMe(ct);
        if (resp.HttpStatusCode == HttpStatusCode.OK)
        {
            Debug.Assert(resp.APIResponse.Code.ResponseId == APIResponseCodeEnum.UserSelfView);
            var data = resp.APIResponse.Data?.Cast<UserSelfViewModel>().FirstOrDefault();
            Debug.Assert(data is not null);
            await state.SetToken(resp.APIResponse.BearerToken);
            state.LoggedInUser = data;
            return;
        }

        nav.NavigateTo("/Login");
    }
}
