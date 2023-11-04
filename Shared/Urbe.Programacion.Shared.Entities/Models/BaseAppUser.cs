using Microsoft.AspNetCore.Identity;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities.Models;

public class BaseAppUser : IdentityUser<Guid>, IEntity, IKeyed<Guid>
{
    public const int EmailMaxLength = 300;
    public const int RealNameMaxLength = 200;
    public const int UserNameMaxLength = 20;
    public const int PronounsMaxLength = 30;
    public const int ProfileMessageMaxLength = 80;
    public const int ProfilePictureUrlMaxLength = 1000;

    public string? RealName { get; set; }

    public string? Pronouns { get; set; }

    private void UpdateEmail(string? value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (string.Equals(base.Email, value, StringComparison.OrdinalIgnoreCase) is false)
        {
            base.Email = value;
            base.NormalizedEmail = value.ToUpper();
        }
    }

    public override string? Email
    {
        get => base.Email;
        set => UpdateEmail(value);
    }

    public override string? NormalizedEmail
    {
        get => base.NormalizedEmail;
        set => UpdateEmail(value);
    }
}
