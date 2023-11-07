using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages
{
    public class PrivacyModel : PageModel
    {
        public const string PrivacyNotice = """
            Le autore de éste sitio no se hace responsable por el uso de sus datos.
            Su contraseña está hasheada a través de un algoritmo apropiado para seguridad y es ilegible por los administradores de ésta aplicación.
            El resto de su información es protegida de acceso indeseado desde ésta aplicación, mas no a nivel de bases de datos.
            """;

        public void OnGet()
        {
        }
    }
}
