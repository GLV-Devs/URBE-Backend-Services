using Urbe.BasesDeDatos.AppSocial.Common;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IUpdateable<TEntity, TUpdateModel> where TEntity : IEntity
{
    public ValueTask<ErrorMessage[]?> Update(TUpdateModel model);
}
