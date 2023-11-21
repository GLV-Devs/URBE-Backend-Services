using Microsoft.AspNetCore.Components;
using Urbe.Programacion.AppSocial.DataTransfer.Responses;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Components
{
    public partial class ContactCard
    {
        [Parameter]
        public UserViewModel? UserModel { get; set; } = new();
    }
}
