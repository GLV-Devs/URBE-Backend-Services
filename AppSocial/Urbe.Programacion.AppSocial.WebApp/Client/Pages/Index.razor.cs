using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Index
{
    public IEnumerable<UserViewModel>? Users;
    public IEnumerable<ErrorMessage>? Errors;
    public bool IsErrored;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            log.LogInformation("Getting User information from server");
            var response = await client.Users.GetUsers();
            log.LogInformation("Got response: HTTP Status: {httpcode}, APIResponse: {apicode}", response.HttpStatusCode, response.APIResponse.Code.ResponseId);
            if (response.HttpStatusCode == HttpStatusCode.OK && response.APIResponse.Code.ResponseId == APIResponseCodeEnum.UserView)
            {
                log.LogInformation("Got user info: {data}", response.APIResponse.Data);
                Users = (IEnumerable<UserViewModel>)response.APIResponse.Data!;
            }
            else
            {
                log.LogError("An error ocurred: {errors}", response.APIResponse.Errors);
                Errors = response.APIResponse.Errors;
                IsErrored = true;
            }

            log.LogInformation("Notifying of state change");
            StateHasChanged();
        }
    }
}
