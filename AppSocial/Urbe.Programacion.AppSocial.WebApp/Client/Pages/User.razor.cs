using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class User
{
    protected UserViewModel? UserModel;
    protected UserUpdateModel UpdateModel { get; } = new();

    protected PostList Posts { get; } = new();

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (await VerifyUserState() is false)
            return;

        var resp = await Client.Users.GetMe();
        if (CheckResponse(resp, APIResponseCodeEnum.UserView) is false)
            return;

        Debug.Assert(resp.APIResponse.Data is not null);

        UserModel = resp.APIResponse.Data.Cast<UserViewModel>().First();

        resp = await Client.Posts.GetUserPosts(UserModel.UserId);
        if (CheckResponse(resp, APIResponseCodeEnum.PostView) is false)
            return;

        Debug.Assert(resp.APIResponse.Data is not null);
        Posts.Add(resp.APIResponse.Data.Cast<PostViewModel>());
    }
}
