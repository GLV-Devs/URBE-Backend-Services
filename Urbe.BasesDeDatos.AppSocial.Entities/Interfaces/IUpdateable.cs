using Urbe.BasesDeDatos.AppSocial.Common;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IUpdateable<TUpdateModel>
{
    public ValueTask<ErrorMessage[]?> Update(TUpdateModel model);
}
