using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Urbe.Programacion.Shared.Services.Attributes;
using System.Security.Cryptography.X509Certificates;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Options;

[RegisterOptions]
public class JWTOptions
{
    public string Key { get; init; }
    public string JWTCertificate { get; init; }
    public string? Authority { get; init; }
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
    public TimeSpan JwtValidty { get; init; }

    private SymmetricSecurityKey? _k;
    public SymmetricSecurityKey SecurityKey => _k ??= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

    private X509Certificate? _c;
    public X509Certificate Certificate => _c ??= new X509Certificate(
        Encoding.UTF8.GetBytes(JWTCertificate)
    );
}
