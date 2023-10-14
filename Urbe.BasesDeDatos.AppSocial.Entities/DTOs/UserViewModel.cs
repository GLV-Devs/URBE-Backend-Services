using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.Entities.DTOs;

public class UserViewModel 
{
    public required GuidId<User> Id { get; set; }
    public required string Username { get; set; }
    public required UserStatus Status { get; set; }
    public required string ProfilePictureUrl { get; set; }
    public string? RealName { get; set; }

    public static UserViewModel FromUser(User user)
        => new()
        {
            Id = user.Id,
            Username = user.Username,
            Status = user.Status,
            ProfilePictureUrl = user.GetPictureOrDefault(),
            RealName = user.RealName
        };
}
