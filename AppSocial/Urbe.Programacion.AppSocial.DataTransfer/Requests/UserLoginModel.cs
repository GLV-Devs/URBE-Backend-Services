using System.ComponentModel.DataAnnotations;

namespace Urbe.Programacion.AppSocial.DataTransfer.Requests;

public class UserLoginModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "El correo o nombre de usuario es requerido")]
    public string? UserNameOrEmail { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "La contraseña es requerida")]
    public string? Password { get; set; }
}
