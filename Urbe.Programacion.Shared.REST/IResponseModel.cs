namespace Urbe.Programacion.Shared.Common;

public interface IResponseModel<TObjectCode>
    where TObjectCode : struct, IEquatable<TObjectCode>
{
    public TObjectCode APIResponseCode { get; }
}
