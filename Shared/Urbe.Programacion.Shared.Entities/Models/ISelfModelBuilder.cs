using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities.Models;

public static class ModelBuilderExtensions
{
    public static void BuildModel<TEntity>(this ModelBuilder mb, DbContext db)
        where TEntity : class, IEntity, ISelfModelBuilder<TEntity>
    {
        TEntity.BuildModel(mb, mb.Entity<TEntity>(), db);
    }

    public static PropertyBuilder<DateTimeOffset?> DateTimeOffsetAsTicksIfSQLite(this PropertyBuilder<DateTimeOffset?> mb, DbContext db)
    {
        if (db.Database.IsSqlite())
            mb.HasConversion(DateTimeOffsetNullableTicksConverter);

        return mb;
    }

    public static PropertyBuilder<DateTimeOffset> DateTimeOffsetAsTicksIfSQLite(this PropertyBuilder<DateTimeOffset> mb, DbContext db)
    {
        if (db.Database.IsSqlite())
            mb.HasConversion(DateTimeOffsetTicksConverter);

        return mb;
    }

    public static ValueConverter<DateTimeOffset?, long?> DateTimeOffsetNullableTicksConverter
        => new(
                  x => x.HasValue ? x.Value.UtcTicks : null,
                  y => y.HasValue ? new DateTimeOffset(y.Value, default) : null // default cuz it's UTC
            );

    public static ValueConverter<DateTimeOffset, long> DateTimeOffsetTicksConverter
        => new(
                  x => x.UtcTicks,
                  y => new DateTimeOffset(y, default) // default cuz it's UTC
            );
}

public interface ISelfModelBuilder<TEntity> where TEntity : class, IEntity
{
    public static abstract void BuildModel(ModelBuilder modelBuilder, EntityTypeBuilder<TEntity> mb, DbContext context);
}
