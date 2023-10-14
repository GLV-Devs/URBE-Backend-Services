using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public class PendingMailConfirmation : IEntity, IKeyed<RandomKey>, ISelfModelBuilder<PendingMailConfirmation>
{
    public RandomKeyId<PendingMailConfirmation> Id { get; init; }
    RandomKey IKeyed<RandomKey>.Id => Id.Value;

    public static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<PendingMailConfirmation> mb)
    {
        throw new NotImplementedException();
    }
}
