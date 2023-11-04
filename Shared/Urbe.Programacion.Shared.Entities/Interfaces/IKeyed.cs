namespace Urbe.Programacion.Shared.Entities.Interfaces;

public interface IKeyed<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; }
}
