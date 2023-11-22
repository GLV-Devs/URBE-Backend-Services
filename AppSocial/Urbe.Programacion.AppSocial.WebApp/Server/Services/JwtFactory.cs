using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Urbe.Programacion.AppSocial.WebApp.Server.Options;
using Urbe.Programacion.Shared.Services.Attributes;

namespace Urbe.Programacion.AppSocial.WebApp.Server.Services;

[RegisterService(Lifetime = ServiceLifetime.Singleton)]
public class JwtFactory
{
    private readonly SymmetricSecurityKey Key;
    private readonly SigningCredentials Credentials;
    private readonly string? Issuer;
    private readonly string? Audience;
    private readonly TimeSpan JwtValidty;
    private readonly JwtSecurityTokenHandler JwtHandler;

    public JwtFactory(IOptions<JWTOptions> options)
    {
        Key = options.Value.SecurityKey;
        Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        JwtValidty = options.Value.JwtValidty;
        Issuer = options.Value.Issuer;
        Audience = options.Value.Audience;

        JwtHandler = new JwtSecurityTokenHandler();
    }

    public SecurityToken CreateToken(ClaimsIdentity claims)
    {
        var now = DateTime.Now;
        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = claims,
            Audience = Audience,
            Issuer = Issuer,
            SigningCredentials = Credentials,
            IssuedAt = now,
            Expires = now + JwtValidty,
            NotBefore = now.AddMinutes(-1)
        };

        return JwtHandler.CreateToken(tokenDesc);
    }

    public string WriteToken(SecurityToken token) 
        => JwtHandler.WriteToken(token);

    public string CreateAndWriteToken(ClaimsIdentity claims)
        => WriteToken(CreateToken(claims));
}
