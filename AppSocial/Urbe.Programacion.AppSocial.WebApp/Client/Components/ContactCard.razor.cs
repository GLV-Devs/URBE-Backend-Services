using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class ContactCard
{
    [Parameter]
    public UserViewModel? UserModel { get; set; } = new();

    public string FollowLabel => UserModel?.IsFollowedByRequester is true ? "Dejar de Seguir" : "Seguir";

    protected async Task FollowUser()
    {
        var user = UserModel;
        if (user is null) return;

        var resp = await Client.Users.FollowUser(user.UserId);
        if (CheckResponse(resp, APIResponseCodeEnum.Success) is false)
        {
            Logger.LogError("An error ocurred while trying to follow user {username}:{uid}", user.Username, user.UserId);
            Logger.LogRequestResponse(resp);
            return;
        }
    }
}
