using Microsoft.AspNetCore.Components;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class Loading
{
    [Parameter]
    public bool LoadingEnabled { get; set; }

    private string LoadingEnabledCssClass => LoadingEnabled ? "inherit" : "none";
}
