using Urbe.Programacion.AppSocial.Entities;
using Urbe.Programacion.AppSocial.Entities.Models;

namespace Urbe.Programacion.AppSocial.ModelServices.DTOs.Requests;

public class PostCreationModel
{
    public required string Content { get; init; }
    public long? InResponseTo { get; init; }
}
