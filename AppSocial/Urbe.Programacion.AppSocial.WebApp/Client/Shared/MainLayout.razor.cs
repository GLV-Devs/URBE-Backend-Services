namespace Urbe.Programacion.AppSocial.WebApp.Client.Shared;

public partial class MainLayout
{
    public async Task Logout()
    {
        await State.SetToken(null);
        Nav.NavigateTo("/Login", true);
    }
}
