using Microsoft.AspNetCore.Identity;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities.Models;

public class BaseAppRole : IdentityRole<Guid>, IEntity, IKeyed<Guid>
{

}

public class BaseAppUser : IdentityUser<Guid>, IEntity, IKeyed<Guid>
{
    public const int EmailMaxLength = 100;
    public const int RealNameMaxLength = 100;
    public const int UserNameMaxLength = 20;
    public const int ProfileMessageMaxLength = 80;
    public const int ProfilePictureUrlMaxLength = 1000;

    public string? RealName { get; set; }

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
