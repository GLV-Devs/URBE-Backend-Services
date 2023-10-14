using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Common;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Internal;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class SocialAppUser : IdentityUser<GuidId<SocialAppUser>>, IEntity, ISelfModelBuilder<SocialAppUser>, IKeyed<Guid>
{
    public const int EmailMaxLength = 300;
    public const int RealNameMaxLength = 200;
    public const int UserNameMaxLength = 20;
    public const int PronounsMaxLength = 30;
    public const int ProfileMessageMaxLength = 80;
    public const int ProfilePictureUrlMaxLength = 1000;

    public string? RealName { get; set; }

    public string? Pronouns { get; set; }

    public string? ProfileMessage { get; set; }

    public UserStatus Status { get; set; }

    public HashSet<SocialAppUser>? Follows { get; set; }

    public HashSet<Post>? Posts { get; set; }

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

    public int FollowerCount { get; init; }

    public string? ProfilePictureUrl { get; set; }

    public override bool PhoneNumberConfirmed 
    { 
        get => true; 
        set { }
    }

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<SocialAppUser> mb)
    {
        mb.HasKey(x => x.Id);

        mb.Property(x => x.Id).HasConversion(GuidId<SocialAppUser>.ValueConverter);
        mb.Property(x => x.Email).HasMaxLength(EmailMaxLength);
        mb.Property(x => x.RealName).HasMaxLength(RealNameMaxLength);
        mb.Property(x => x.UserName).HasMaxLength(UserNameMaxLength).IsRequired(true).HasDefaultValue($"User{Random.Shared.Next(100_000_000, 999_999_999)}");
        mb.Property(x => x.Pronouns).HasMaxLength(PronounsMaxLength);
        mb.Property(x => x.ProfilePictureUrl).HasMaxLength(ProfilePictureUrlMaxLength);
        mb.Property(x => x.ProfileMessage).HasMaxLength(ProfileMessageMaxLength);

        mb.HasMany(x => x.Follows).WithMany();
    }

    Guid IKeyed<Guid>.Id => Id.Value;
}
