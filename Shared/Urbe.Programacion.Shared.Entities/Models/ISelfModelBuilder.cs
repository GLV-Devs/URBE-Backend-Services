using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities.Models;

public static class SelfModelBuilderExtensions
{
    public static void BuildModel<TEntity>(this ModelBuilder mb)
        where TEntity : class, IEntity, ISelfModelBuilder<TEntity>
    {
        TEntity.BuildModel(mb, mb.Entity<TEntity>());
    }
}

public interface ISelfModelBuilder<TEntity> where TEntity : class, IEntity
{
    public static abstract void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> mb);
}
