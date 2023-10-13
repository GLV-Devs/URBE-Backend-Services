namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IKeyed<TKey> where TKey : struct
{
    public TKey Id { get; }
}
