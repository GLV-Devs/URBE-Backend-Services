using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities.Models;

public interface ISelfModelBuilder<TEntity> where TEntity : class, IEntity
{
    public static abstract void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> mb);
}
