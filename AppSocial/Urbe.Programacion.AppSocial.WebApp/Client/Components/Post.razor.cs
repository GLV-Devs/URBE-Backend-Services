using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class Post
{
    [Parameter]
    public PostViewModel? PostData { get; set; }

    [Parameter]
    public PostList? PostList { get; set; }

    public async Task DeletePost()
    {
        if (PostData?.Id is not long id)
            return;

        await Client.Posts.Delete(new Snowflake(id));

        PostList?.RequestRefresh();
    }

    public void GoToUser()
    {
        if (string.IsNullOrWhiteSpace(PostData?.Poster?.Username)) return;

        Nav.NavigateTo($@"/User/{PostData.Poster.Username}", true);
    }
}
