using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities.Internal;

public class NullableKeyedNavigation<TId, TEntity>
    where TId : struct, IEquatable<TId>
    where TEntity : class, IEntity, IKeyed<TId>
{
    private TId? id;
    private TEntity? entity;

    public TId? Id
    {
        get => id;
        set
        {
            if (value is null || id.Equals(value) is false || Entity?.Id.Equals(value) is false)
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
            id = value != null ? value?.Id : default;
        }
    }
}
