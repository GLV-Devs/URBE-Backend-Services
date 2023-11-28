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
        Logger.LogInformation("Creating post");
        var resp = await Client.Posts.Create(CreationModel);
        if (resp.HttpStatusCode != HttpStatusCode.OK)
        {
            Logger.LogInformation("Post could not be created due to an HTTP error: {httpcode}", resp.HttpStatusCode);
            if (resp.APIResponse?.Errors is IEnumerable<ErrorMessage> msgs)
                Errors.AddErrorRange(msgs);
            else
                Errors.AddError(ErrorMessages.InvalidServerHttpCode(resp.HttpStatusCode));

            return;
        }

        if (resp.APIResponse.Code.ResponseId is not APIResponseCodeEnum.PostView)
        {
            Logger.LogInformation("Post could not be created due to an unexpected API Response: {apicode}", resp.APIResponse.Code.ResponseId.ToString());
            Logger.LogRequestResponse(resp);

            if (resp.APIResponse.Errors is IEnumerable<ErrorMessage> msgs)
                Errors.AddErrorRange(msgs);
            else
                Errors.AddError(ErrorMessages.UnexpectedServerResponse(
                    (int)resp.APIResponse.Code.ResponseId, 
                    resp.APIResponse.Code.ResponseId.ToString()
                ));

            return;
        }

        Debug.Assert(resp.APIResponse.Data is not null);
        Logger.LogInformation("Post succesfully created");
        PostList?.Add(resp.APIResponse.Data.Cast<PostViewModel>());
    }
}
