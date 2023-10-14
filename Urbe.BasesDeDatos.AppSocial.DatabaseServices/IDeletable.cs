namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IDeletable
{
    public ValueTask<bool> Delete(SocialContext context)
    {
        context.Remove(this);
        return ValueTask.FromResult(true);
    }
}
