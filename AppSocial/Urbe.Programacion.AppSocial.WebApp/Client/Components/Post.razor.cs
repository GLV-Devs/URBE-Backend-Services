using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;
using Urbe.Programacion.AppSocial.WebApp.Client.Data;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class Post
{
    [Parameter]
    public PostViewModel? PostData { get; set; }

    [Parameter]
    public PostList? PostList { get; set; }
}
