using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;

public class UserViewModel
{
    public required GuidId<SocialAppUser> Id { get; set; }
    public required string Username { get; set; }
    public required UserStatus Status { get; set; }
    public required string? ProfilePictureUrl { get; set; }
    public string? RealName { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }

    public static UserViewModel FromUser(SocialAppUser user)
        => new()
        {
            Id = user.Id,
            Username = user.UserName!,
            Status = user.Status,
            ProfilePictureUrl = user.ProfilePictureUrl,
            RealName = user.RealName
        };
}
