using System.ComponentModel.DataAnnotations;
using Urbe.Programacion.AppSocial.DataTransfer.Requests;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.WebApp.Client.Data;

public class ClientUserCreationModel : UserCreationModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorMessages.EmptyProperty))]
    public string? ConfirmEmail { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorMessages.EmptyProperty))]
    public string? ConfirmPassword { get; set; }
}
