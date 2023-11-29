using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Components;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class MyAccount
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

    protected UserSelfViewModel? SelfModel;
    protected UserUpdateModel UpdateModel { get; } = new();
    protected ErrorList Errors;

    private bool AllowRealNamePublicly;

    private bool AllowNonFollowerViews;
    private bool AllowAnonymousViews;

    private bool AllowNonFollowerPostViews;
    private bool AllowAnonymousPostViews;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (await State.VerifyUserState(Client, Nav, Log) is false)
            return;

        SelfModel = State.LoggedInUser!;
        var s = SelfModel.Settings;

        AllowRealNamePublicly = s.HasFlag(UserSettings.AllowRealNamePublicly);
        AllowNonFollowerViews = s.HasFlag(UserSettings.AllowNonFollowerViews);
        AllowAnonymousViews = s.HasFlag(UserSettings.AllowAnonymousViews);
        AllowNonFollowerPostViews = s.HasFlag(UserSettings.AllowNonFollowerPostViews);
        AllowAnonymousPostViews = s.HasFlag(UserSettings.AllowAnonymousPostViews);
    }

    public async Task ValidSubmit()
    {
        Errors.Clear();
        IsLoading = true;
        await Task.Yield();
        try
        {
            UpdateModel.UserSettings = 0;
            if (AllowRealNamePublicly) UpdateModel.UserSettings |= UserSettings.AllowRealNamePublicly;
            if (AllowNonFollowerViews) UpdateModel.UserSettings |= UserSettings.AllowNonFollowerViews;
            if (AllowAnonymousViews) UpdateModel.UserSettings |= UserSettings.AllowAnonymousViews;
            if (AllowNonFollowerPostViews) UpdateModel.UserSettings |= UserSettings.AllowNonFollowerPostViews;
            if (AllowAnonymousPostViews) UpdateModel.UserSettings |= UserSettings.AllowAnonymousPostViews;

            Log.LogInformation("Attempting to modify user");
            var resp = await Client.Users.Update(UpdateModel);
            if (Helper.IsExpectedCode(ref Errors, resp.HttpStatusCode) is false)
            {
                Log.LogInformation("An error ocurred modifying the user");
                Log.LogRequestResponse(resp);
                StateHasChanged();
                return;
            }

            Log.LogInformation("Succesfully modified user");
            Log.LogRequestResponse(resp);

            Nav.NavigateTo("/MyAccount");
        }
        finally
        {
            IsLoading = false;
            await Task.Yield();
        }
    }
}
