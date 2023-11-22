using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Session;
using Urbe.Programacion.AppSocial.WebApp.Server.Services;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Authentication;

public class JwtSignInHandlerOptions : JwtBearerOptions
{
    private JwtFactory? tokenFactor;

    public JwtFactory TokenFactory 
    { 
        get => tokenFactor!; 
        set => tokenFactor = value ?? throw new ArgumentNullException(nameof(value)); 
    }
    
    public override void Validate()
    {
        base.Validate();
        if (TokenFactory is null)
            throw new InvalidOperationException("TokenFactory cannot remain null");
    }
}
