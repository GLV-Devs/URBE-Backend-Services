using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Index
{
    public IEnumerable<PostViewModel>? Posts;
    public ErrorList Errors;
    public bool IsErrored;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await State.VerifyUserState(Client, Nav);
            await RefreshFeed();
        }
    }

    protected async Task RefreshFeed()
    {
        var x = await Client.Posts.GetFeed();
        if (Helper.IsExpectedCode(ref Errors, x.HttpStatusCode) is false
            || x.APIResponse.Code.IsExpectedResponse(ref Errors, APIResponseCodeEnum.PostView) is false)
        {
            if (x.APIResponse.Errors is not null)
                Errors.AddErrorRange(x.APIResponse.Errors);
            return;
        }

        Debug.Assert(x.APIResponse.Data is not null);
        Posts = x.APIResponse.Data.Cast<PostViewModel>();
    }
}
