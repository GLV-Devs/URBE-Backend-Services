namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IDeletable
{
    public abstract ValueTask<bool> Delete(SocialContext context);
}
