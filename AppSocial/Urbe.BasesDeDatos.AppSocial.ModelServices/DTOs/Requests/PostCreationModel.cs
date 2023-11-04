using Urbe.BasesDeDatos.AppSocial.Entities;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.Programacion.AppSocial.ModelServices.DTOs.Requests;

public class PostCreationModel
{
    public required string Content { get; init; }
    public long? InResponseTo { get; init; }
}
