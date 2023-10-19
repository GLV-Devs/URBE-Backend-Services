﻿using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;
using Urbe.BasesDeDatos.AppSocial.ModelServices.API.Responses;

namespace Urbe.BasesDeDatos.AppSocial.ModelServices.DTOs.Responses;

public class UserSelfViewModel : IResponseModel
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required UserSettings Settings { get; set; }
    public required string? ProfilePictureUrl { get; set; }
    public string? RealName { get; set; }
    public string? Pronouns { get; set; }
    public string? ProfileMessage { get; set; }

    public static UserSelfViewModel FromUser(SocialAppUser user)
        => new()
        {
            Id = user.Id,
            Username = user.UserName!,
            Settings = user.Settings,
            ProfilePictureUrl = user.ProfilePictureUrl,
            RealName = user.RealName
        };

    APIResponseCode IResponseModel.APIResponseCode => APIResponseCodeEnum.UserSelfView;
}
