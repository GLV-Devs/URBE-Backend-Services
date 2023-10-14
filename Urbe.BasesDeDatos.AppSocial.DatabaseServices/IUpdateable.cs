using Urbe.BasesDeDatos.AppSocial.Common;

namespace Urbe.BasesDeDatos.AppSocial.DatabaseServices;

public interface IUpdateable<TUpdateModel>
{
    public ValueTask<ErrorList> Update(TUpdateModel model);
}
