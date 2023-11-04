namespace Urbe.Programacion.Shared.ModelServices;

public interface IAPIResponseObjectCode<TObjectCode>
    where TObjectCode : struct, IEquatable<TObjectCode>, IAPIResponseObjectCode<TObjectCode>
{
    public static abstract TObjectCode NoData { get; }
    public static abstract TObjectCode ErrorCollection { get; }
    public static abstract TObjectCode Success { get; }
    public static abstract TObjectCode UnspecifiedError { get; }
    public static abstract TObjectCode Exception { get; }
}
