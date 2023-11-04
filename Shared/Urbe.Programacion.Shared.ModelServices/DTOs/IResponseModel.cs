namespace Urbe.Programacion.Shared.ModelServices.DTOs;

public interface IResponseModel<TObjectCode>
    where TObjectCode : struct, IEquatable<TObjectCode>
{
    public TObjectCode APIResponseCode { get; }
}
