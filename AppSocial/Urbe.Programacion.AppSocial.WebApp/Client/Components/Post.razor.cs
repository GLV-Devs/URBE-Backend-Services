using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class Post
{
    [Parameter]
    public PostViewModel? PostData { get; set; }
}
