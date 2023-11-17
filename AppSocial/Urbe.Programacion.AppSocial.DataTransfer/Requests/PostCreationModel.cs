namespace Urbe.Programacion.AppSocial.DataTransfer.Requests;

public class PostCreationModel
{
    public required string Content { get; init; }
    public long? InResponseTo { get; init; }
}
