using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class NewPost
{
    [Parameter]
    public PostViewModel? InResponseTo { get; set; }

    public string PlaceholderText
        => InResponseTo is null ? "Escribe una nueva publicación..." : "Escribe una respuesta...";
    
    public PostCreationModel CreationModel { get; } = new();

    public async Task ValidSubmit()
    {
        CreationModel.InResponseTo = InResponseTo?.Id;
    }
}
