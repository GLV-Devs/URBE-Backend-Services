using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Urbe.Programacion.AppSocial.ClientLibrary;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Login
{
    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            if (value != isLoading)
            {
                isLoading = value;
                StateHasChanged();
            }
        }
    }

    public UserLoginModel LoginModel { get; } = new();
    protected ErrorList Errors;

    public async Task ValidSubmit()
    {
        Errors.Clear();
        IsLoading = true;
        await Task.Yield();
        try
        {
            Log.LogInformation("Attempting to log in user");
            var resp = await Client.Identity.LogIn(LoginModel);
            if (Helper.IsExpectedCode(ref Errors, resp.HttpStatusCode) is false)
            {
                Log.LogInformation("An error ocurred logging the user in");
                Log.LogRequestResponse(resp);
                StateHasChanged();
                return;
            }

            if (resp.APIResponse.Code.IsExpectedResponse(ref Errors, APIResponseCodeEnum.UserSelfView))
            {
                Log.LogInformation("An error ocurred logging the user in");
                Log.LogRequestResponse(resp);
                StateHasChanged();
                return;
            }

            var u = resp.APIResponse.Data?.Cast<UserSelfViewModel>().FirstOrDefault();

            if (u is null)
            {
                Errors.AddError(ErrorMessages.InternalError("El servidor no retorno data de un usuario"));
                Log.LogInformation("An error ocurred logging the user in");
                Log.LogRequestResponse(resp);
                StateHasChanged();
                return;
            }

            Log.LogInformation("Succesfully logged in");
            Log.LogRequestResponse(resp);
            await State.SetToken(resp.APIResponse.BearerToken);
            State.LoggedInUser = u;

            Nav.NavigateTo("/");
        }
        finally
        {
            IsLoading = false;
            await Task.Yield();
        }
    }
}
