namespace Urbe.Programacion.Shared.Entities.Interfaces;

public class ModifiableEntity
{
    public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.Now;
    public DateTimeOffset LastModified { get; set; }

    protected void NotifyModified()
    {
        LastModified = DateTimeOffset.Now;
    }
}
