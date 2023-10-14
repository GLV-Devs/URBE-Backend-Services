using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

public interface IReadable
{
    public ValueTask<object> GetView();
}
