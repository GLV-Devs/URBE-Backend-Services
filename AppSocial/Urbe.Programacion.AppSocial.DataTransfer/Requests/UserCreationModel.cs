using System.ComponentModel.DataAnnotations;
using Urbe.Programacion.Shared.Common;

namespace Urbe.Programacion.AppSocial.DataTransfer.Requests;

public class UserCreationModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorMessages.BadUsername))]
    public string? Username { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorMessages.BadEmail))]
    public string? Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = nameof(ErrorMessages.BadPassword))]
    public string? Password { get; set; }

    [StringLength(30, ErrorMessage = "Este campo tiene un maximo de 30 caracteres")]
    public string? Pronouns { get; set; }

    [StringLength(100, ErrorMessage = "Este campo tiene un maximo de 100 caracteres")]
    public string? RealName { get; set; }
}
