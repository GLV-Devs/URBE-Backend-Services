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

        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }

    private static void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.State is EntityState.Modified && e.Entry.Entity is ModifiableEntity modifiable)
            modifiable.LastModified = DateTimeOffset.Now;
    }
}
