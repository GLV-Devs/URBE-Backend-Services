using System.Diagnostics;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Pages;

public partial class Index
{
    public PostList Posts = new();
    public ErrorList Errors;
    public bool IsErrored;

    public Index()
    {
        Posts.ListCleared += Posts_ListCleared;
        Posts.PostAdded += Posts_PostAdded;
    }

    private void Posts_PostAdded(PostList arg1, PostViewModel arg2)
    {
        StateHasChanged();
    }

    private void Posts_ListCleared(PostList obj)
    {
        StateHasChanged();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            if (await State.VerifyUserState(Client, Nav, Log) is false)
                return;
            await RefreshFeed();
        }
    }

    protected async Task RefreshFeed()
    {
        Errors.Clear();
        var x = await Client.Posts.GetFeed();
        //if (Helper.IsExpectedCode(ref Errors, x.HttpStatusCode) is false 
        //    || x.APIResponse.Code.IsExpectedResponse(ref Errors, APIResponseCodeEnum.PostView) is false)
        //{
        //    Log.LogRequestResponse(x);
        //    if (x.APIResponse.Errors is not null)
        //        Errors.AddErrorRange(x.APIResponse.Errors);
        //    return;
        //}

        Debug.Assert(x.APIResponse.Data is not null);
        Posts.Clear();
        Posts.Add(x.APIResponse.Data.Cast<PostViewModel>());
    }
}
