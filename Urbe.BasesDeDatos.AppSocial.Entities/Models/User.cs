using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class User : ModifiableEntity, IKeyed<Guid>, IEntity, ISelfModelBuilder<User>, ICRUDEntity<User, UserUpdateModel, UserCreationModel>
{
    public const int EmailMaxLength = 300;
    public const int RealNameMaxLength = 200;
    public const int UserNameMaxLength = 20;
    public const int PronounsMaxLength = 30;
    public const int ProfileMessageMaxLength = 80;

    private string? username;
    private string email;

    public User(GuidId<User> id, string? realName, string username, string email, byte[] passwordHash, byte[] passwordSalt, UserStatus status, HashSet<User>? follows, string? profilePictureUrl)
    {
        Id = id;
        RealName = realName;
        Username = username ?? throw new ArgumentNullException(nameof(username));
        this.email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        PasswordSalt = passwordSalt ?? throw new ArgumentNullException(nameof(passwordSalt));
        Status = status;
        Follows = follows;
        ProfilePictureUrl = profilePictureUrl;
    }

    public GuidId<User> Id { get; init; }

    public string? RealName { get; set; }

    public string? Pronouns { get; set; }

    public string? ProfileMessage { get; set; }

    [MemberNotNull(nameof(username))]
    public string Username
    {
        get
        {
            if (username is null)
            {
                var bstr = RealName ?? "User";
                Username = $"{bstr[..int.Min(bstr.Length, UserNameMaxLength - 9)]}{Random.Shared.Next(100000000, 999999999)}";
                // The -9 comes from the fact that the bunch of numbers are a value between 100 000 000 and 999 999 999; always constrained within 9 digits in decimal (the system used)
            }
            return username;
        }

        set
        {
            if (value is null)
                throw new ArgumentException("An user's username cannot be set to null");

            if (username != value)
            {
                username = value;
                NotifyModified();
            }
        }
    }

    public string Email 
    { 
        get => email;
        set
        {
            if (value is null)
                throw new ArgumentException("An user's email cannot be set to null");

            if (email != value)
            {
                email = value;
                NotifyModified();
            }
        }
    }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public UserStatus Status { get; set; }

    public HashSet<User>? Follows { get; set; }

    public HashSet<Post>? Posts { get; set; }

    public int FollowerCount { get; init; }

    public string? ProfilePictureUrl { get; set; }

    public string GetPictureOrDefault()
        => ProfilePictureUrl ?? throw new NotImplementedException("Default profile pictures are not yet supported");

    public IQueryable<User> GetFollowers(SocialContext context)
        => context.Users.Where(x => x.Follows!.Contains(this));

    Guid IKeyed<Guid>.Id => Id.Value;

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<User> mb)
    {
        mb.HasKey(x => x.Id);
        
        mb.Property(x => x.Id).HasConversion(GuidId<User>.ValueConverter);
        mb.Property(x => x.Email).HasMaxLength(EmailMaxLength);
        mb.Property(x => x.RealName).HasMaxLength(RealNameMaxLength);
        mb.Property(x => x.Username).HasMaxLength(UserNameMaxLength);
        mb.Property(x => x.Pronouns).HasMaxLength(PronounsMaxLength);
        mb.Property(x => x.ProfileMessage).HasMaxLength(ProfileMessageMaxLength);
        
        mb.HasMany(x => x.Follows).WithMany();

        //mb.Property(x => x.FollowerCount)
        //    .HasComputedColumnSql("COUNT(*) FROM Users WHERE U");
    }
}
