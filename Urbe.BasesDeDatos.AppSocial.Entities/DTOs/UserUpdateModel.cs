﻿namespace Urbe.BasesDeDatos.AppSocial.Entities.DTOs;

public class UserUpdateModel 
{
    public string? RealName { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfilePictureUrl { get; set; }
}
