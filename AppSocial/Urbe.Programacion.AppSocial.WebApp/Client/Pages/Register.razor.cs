using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Register
{
    public ClientUserCreationModel CreationModel { get; } = new();

    public async Task ValidSubmit()
    {
        Errors.Clear();

        if (CreationModel.ConfirmPassword != CreationModel.Password)
            Errors.AddError(ErrorMessages.PropertiesNotEqual("Contraseña", "Confirmación de Contraseña"));

        if (CreationModel.ConfirmEmail != CreationModel.Email)
            Errors.AddError(ErrorMessages.PropertiesNotEqual("Correo Electrónico", "Confirmación de Correo Electrónico"));

        if (Errors.Count > 0)
        {
            StateHasChanged();
            return;
        }

        var resp = await Client.Identity.CreateNew(CreationModel);

        var u = resp.APIResponse.Data?.Cast<UserSelfViewModel>().FirstOrDefault();

        if (u is null)
        {
            Errors.AddError(ErrorMessages.InternalError("Ocurrió un error al crear el usuario"));
            if (resp.APIResponse.Errors is not null)
                Errors.AddErrorRange(resp.APIResponse.Errors);
            StateHasChanged();
            return;
        }

        await AppState.SetToken(resp.APIResponse.BearerToken);
        AppState.LoggedInUser = u;
        Navigation.NavigateTo("/");
    }
}
