using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.Shared.Entities;

public abstract class BaseAppContext : DbContext
{
    private static readonly object migrationsync = new();
    private static bool migrated = false;

    public BaseAppContext(DbContextOptions options) : base(options)
    {
        if (migrated is false)
            lock (migrationsync)
                if (migrated is false)
                {
                    Database.Migrate();
                    migrated = true;
                }
    }

    private void UpdateEntities()
    {
        if (ChangeTracker.HasChanges())
        {
            foreach (var i in ChangeTracker.Entries()
                    .Where(x => x.State is EntityState.Modified or EntityState.Added && x.Entity is ModifiableEntity)
                    .Select(x => (ModifiableEntity)x.Entity))
                i.LastModified = DateTimeOffset.Now;
        }
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateEntities();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateEntities();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
