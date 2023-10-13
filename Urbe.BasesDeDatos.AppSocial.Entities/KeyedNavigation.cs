using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

internal class KeyedNavigation<TId, TEntity>
    where TId : struct, IEquatable<TId>
    where TEntity : class, IEntity, IKeyed<TId>
{
    private TId id;
    private TEntity? entity;

    public TId Id
    {
        get => id;
        set
        {
            if (id.Equals(value) is false || Entity?.Id.Equals(value) is false)
                Entity = null;
            id = value;
        }
    }

    public TEntity? Entity
    {
        get => entity;
        set
        {
            entity = value;
            id = value != null ? value.Id : default;
        }
    }
}
