using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.Programacion.AppSocial.Entities.Interfaces;

namespace Urbe.Programacion.AppSocial.Entities.Models;

public interface ISelfModelBuilder<TEntity> where TEntity : class, IEntity
{
    public abstract static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> mb);
}
