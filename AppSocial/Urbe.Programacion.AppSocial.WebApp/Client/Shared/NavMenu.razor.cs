using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Shared;

public partial class NavMenu
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

    public IEnumerable<UserViewModel>? UserList { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        IsLoading = true;
        await Task.Yield();
        try
        {
            Logger.LogInformation("Loading User List");
            var resp = await Client.Users.GetUsers();
            if (resp.HttpStatusCode != HttpStatusCode.OK)
            {
                Logger.LogInformation("User List could not be loaded due to an HTTP error: {httpcode}", resp.HttpStatusCode);
                Logger.LogRequestResponse(resp);
                return;
            }

            if (resp.APIResponse.Code.ResponseId is not APIResponseCodeEnum.UserView)
            {
                Logger.LogInformation("User List could not be loaded due to an unexpected API Response: {apicode}", resp.APIResponse.Code.ResponseId.ToString());
                Logger.LogRequestResponse(resp);

                return;
            }

            Debug.Assert(resp.APIResponse?.Data is not null);
            UserList = resp.APIResponse.Data.Cast<UserViewModel>();
            Logger.LogInformation("User List succesfully loaded");
        }
        finally
        {
            IsLoading = false;
            await Task.Yield();
        }
    }
}
