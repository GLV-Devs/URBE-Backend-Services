using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices.DTOs;

public class PostCreationModel
{
    public required string Content { get; init; }
    public SnowflakeId<Post>? InResponseTo { get; init; }
}
