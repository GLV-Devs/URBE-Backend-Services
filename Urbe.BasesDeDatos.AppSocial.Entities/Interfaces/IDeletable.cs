namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IDeletable
{
    public ValueTask<bool> Delete(SocialContext context)
    {
        context.Remove(this);
        return ValueTask.FromResult(true);
    }
}
