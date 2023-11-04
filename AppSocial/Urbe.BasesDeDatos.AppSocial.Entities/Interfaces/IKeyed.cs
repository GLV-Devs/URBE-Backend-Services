namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IKeyed<TKey> where TKey : struct, IEquatable<TKey>
{
    public TKey Id { get; }
}
