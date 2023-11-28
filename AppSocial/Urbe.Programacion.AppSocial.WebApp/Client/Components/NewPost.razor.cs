using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class NewPost
{
    [Parameter]
    public PostList? PostList { get; set; }

    [Parameter]
    public PostViewModel? InResponseTo { get; set; }

    public string PlaceholderText
        => InResponseTo is null ? "Escribe una nueva publicación..." : "Escribe una respuesta...";
    
    public PostCreationModel CreationModel { get; } = new();

    public async Task ValidSubmit()
    {
        CreationModel.InResponseTo = InResponseTo?.Id;
        var resp = await Client.Posts.Create(CreationModel);
        if (resp.HttpStatusCode != HttpStatusCode.OK)
        {
            if (resp.APIResponse?.Errors is IEnumerable<ErrorMessage> msgs)
                Errors.AddErrorRange(msgs);
            else
                Errors.AddError(ErrorMessages.InvalidServerHttpCode(resp.HttpStatusCode));

            return;
        }

        if (resp.APIResponse.Code.ResponseId is not APIResponseCodeEnum.PostView)
        {
            if (resp.APIResponse.Errors is IEnumerable<ErrorMessage> msgs)
                Errors.AddErrorRange(msgs);
            else
                Errors.AddError(ErrorMessages.UnexpectedServerResponse((int)APIResponseCodeEnum.PostView, APIResponseCodeEnum.PostView.ToString()));

            return;
        }

        Debug.Assert(resp.APIResponse.Data is not null);
        PostList?.Add(resp.APIResponse.Data.Cast<PostViewModel>());
    }
}
