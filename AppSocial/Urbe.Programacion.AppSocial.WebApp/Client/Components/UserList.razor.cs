using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components;

public partial class UserList
{
    [Parameter]
    public IEnumerable<UserViewModel>? UserViews { get; set; }

    [Parameter]
    public bool Compact { get; set; }

    public string CssClass => Compact ? "user-list-compact" : "user-list-full";
}
