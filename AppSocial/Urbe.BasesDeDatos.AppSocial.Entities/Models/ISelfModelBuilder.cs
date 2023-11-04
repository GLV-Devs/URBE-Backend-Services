using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;

namespace Urbe.BasesDeDatos.AppSocial.Entities.Models;

public interface ISelfModelBuilder<TEntity> where TEntity : class, IEntity
{
    public abstract static void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> mb);
}
