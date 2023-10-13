using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class User : ModifiableEntity, IKeyed<Guid>
{
    private string? username;
    private string? email;

    public Guid Id { get; init; }

    public string? RealName { get; set; }

    public string Username
    {
        get
        {
            if (username is null)
            {
                username = $"{(RealName ?? "User")}{Random.Shared.Next(100000, 999999)}";
                NotifyModified();
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

    public string? ProfilePictureUrl { get; set; }

    public string GetPictureOrDefault()
        => ProfilePictureUrl ?? throw new NotImplementedException("Default profile pictures are not yet supported");

    public IQueryable<User> GetFollowers(SocialContext context)
        => context.Users.Where(x => x.Follows!.Contains(this));
}
