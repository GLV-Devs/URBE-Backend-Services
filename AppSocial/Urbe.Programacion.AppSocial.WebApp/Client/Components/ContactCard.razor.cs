using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class ContactCard
{
    [Parameter]
    public UserViewModel? UserModel { get; set; } = new();

    public string FollowLabel => UserModel?.IsFollowedByRequester is true ? "Dejar de Seguir" : "Seguir";
    public string PronounsSeparator => string.IsNullOrWhiteSpace(UserModel?.Pronouns) ? "" : " | ";

    protected async Task FollowUser()
    {
        var user = UserModel;
        if (user is null) return;

        if (user.IsFollowedByRequester)
        {
            var resp = await Client.Users.UnfollowUser(user.UserId);
            if (resp.HttpStatusCode is not HttpStatusCode.OK)
            {
                Logger.LogError("An error ocurred while trying to unfollow user {username}:{uid}", user.Username, user.UserId);
                Logger.LogRequestResponse(resp);
                return;
            }

            //Invariably of the API Result, the OK status represents a succesful operation. If the user was already being followed, the API response would contain errors, but regardless, IsFollowedByRequester needs updating; and the API did its work succesfully
            user.IsFollowedByRequester = false;
        }
        else
        {
            var resp = await Client.Users.FollowUser(user.UserId);
            if (resp.HttpStatusCode is not HttpStatusCode.OK) 
            {
                Logger.LogError("An error ocurred while trying to follow user {username}:{uid}", user.Username, user.UserId);
                Logger.LogRequestResponse(resp);
                return;
            }

            //Invariably of the API Result, the OK status represents a succesful operation. If the user was already being followed, the API response would contain errors, but regardless, IsFollowedByRequester needs updating; and the API did its work succesfully
            user.IsFollowedByRequester = true;
        }
    }
}
