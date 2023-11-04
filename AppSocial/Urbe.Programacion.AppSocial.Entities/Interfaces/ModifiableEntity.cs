using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Urbe.Programacion.AppSocial.Entities.Interfaces;

public class ModifiableEntity
{
    public DateTimeOffset CreatedDate { get; init; } = DateTimeOffset.Now;
    public DateTimeOffset LastModified { get; set; }

    protected void NotifyModified()
    {
        LastModified = DateTimeOffset.Now;
    }
}
