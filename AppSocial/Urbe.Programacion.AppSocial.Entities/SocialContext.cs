using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Entities.Interfaces;

namespace Urbe.Programacion.AppSocial.Entities;

public class SocialContext : DbContext
{
    private static readonly object migrationsync = new();
    private static bool migrated = false;

    public SocialContext(DbContextOptions<SocialContext> options) : base(options)
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

    public DbSet<SocialAppUser> SocialAppUsers => Set<SocialAppUser>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PendingMailConfirmation> PendingMailConfirmations => Set<PendingMailConfirmation>();
    public DbSet<SocialAppUserFollow> SocialAppUserFollows => Set<SocialAppUserFollow>();

    private static void ChangeTracker_StateChanged(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
    {
        if (e.Entry.State is EntityState.Modified && e.Entry.Entity is ModifiableEntity modifiable)
            modifiable.LastModified = DateTimeOffset.Now;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SocialAppUser.BuildModel(modelBuilder, modelBuilder.Entity<SocialAppUser>());
        PendingMailConfirmation.BuildModel(modelBuilder, modelBuilder.Entity<PendingMailConfirmation>());
        Post.BuildModel(modelBuilder, modelBuilder.Entity<Post>());

        var iucmb = modelBuilder.Entity<IdentityUserClaim<Guid>>();
        iucmb.HasKey(x => x.Id);
        iucmb.HasOne(typeof(SocialAppUser)).WithMany().HasForeignKey(nameof(IdentityUserClaim<Guid>.UserId));
    }
}
