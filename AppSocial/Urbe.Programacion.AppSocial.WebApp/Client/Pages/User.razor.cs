using System.Diagnostics;
using Microsoft.AspNetCore.Components;
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

    [Parameter]
    public string? Username { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();

        if (Username is null)
        {
            Navigation.NavigateTo("/");
            return;
        }

        if (await VerifyUserState() is false)
            return;

        var resp = await Client.Users.GetUser(Username);
        //if (CheckResponse(resp, APIResponseCodeEnum.UserView) is false)
        //    return;

        Debug.Assert(resp.APIResponse.Data is not null);

        UserModel = resp.APIResponse.Data.Cast<UserViewModel>().First();

        resp = await Client.Posts.GetUserPosts(UserModel.UserId);

        Debug.Assert(resp.APIResponse.Data is not null);
        Posts.Add(resp.APIResponse.Data.Cast<PostViewModel>());
    }
}
