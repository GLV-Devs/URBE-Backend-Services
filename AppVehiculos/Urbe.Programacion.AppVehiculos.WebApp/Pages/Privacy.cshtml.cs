using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Urbe.Programacion.AppVehiculos.WebApp.Pages
{
    public class PrivacyModel : PageModel
    {
        public const string PrivacyNotice = """
            Le autore de �ste sitio no se hace responsable por el uso de sus datos.
            Su contrase�a est� hasheada a trav�s de un algoritmo apropiado para seguridad y es ilegible por los administradores de �sta aplicaci�n.
            El resto de su informaci�n es protegida de acceso indeseado desde �sta aplicaci�n, mas no a nivel de bases de datos.
            """;

        public void OnGet()
        {
        }
    }
}
