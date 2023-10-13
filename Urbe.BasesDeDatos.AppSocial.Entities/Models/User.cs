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

public class User : ModifiableEntity, IKeyed<Guid>, IEntity, ISelfModelBuilder<User>
{
    private string? username;
    private string? email;

    public User(GuidId<User> id, string? realName, string username, string? email, string passwordHash, string passwordSalt, UserStatus status, HashSet<User>? follows, string? profilePictureUrl)
    {
        Id = id;
        RealName = realName;
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Email = email;
        PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
        PasswordSalt = passwordSalt ?? throw new ArgumentNullException(nameof(passwordSalt));
        Status = status;
        Follows = follows;
        ProfilePictureUrl = profilePictureUrl;
    }

    public GuidId<User> Id { get; init; }

    public string? RealName { get; set; }

    [MemberNotNull(nameof(username))]
    public string Username
    {
        get
        {
            if (username is null)
                Username = $"{(RealName ?? "User")}{Random.Shared.Next(100000, 999999)}";
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

    public string? Email 
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

    public string PasswordHash { get; set; }

    public string PasswordSalt { get; set; }

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
        
        mb.HasMany(x => x.Follows).WithMany();

        //mb.Property(x => x.FollowerCount)
        //    .HasComputedColumnSql("COUNT(*) FROM Users WHERE U");
    }
}
