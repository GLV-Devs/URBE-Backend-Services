using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Urbe.Programacion.AppSocial.ClientLibrary;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Login
{
    public UserLoginModel LoginModel { get; } = new();
    protected ErrorList Errors;

    public async Task ValidSubmit()
    {
        Errors.Clear();

        var resp = await Client.Identity.LogIn(LoginModel);
        if (Helper.IsExpectedCode(ref Errors, resp.HttpStatusCode) is false)
        {
            StateHasChanged();
            return;
        }

        if (resp.APIResponse.Code.IsExpectedResponse(ref Errors, APIResponseCodeEnum.UserSelfView))
        {
            StateHasChanged();
            return;
        }

        var u = resp.APIResponse.Data?.Cast<UserSelfViewModel>().FirstOrDefault();

        if (u is null)
        {
            Errors.AddError(ErrorMessages.InternalError("El servidor no retorno data de un usuario"));
            StateHasChanged();
            return;
        }

        await State.SetToken(resp.APIResponse.BearerToken);
        State.LoggedInUser = u;
        Nav.NavigateTo("/");
    }
}
