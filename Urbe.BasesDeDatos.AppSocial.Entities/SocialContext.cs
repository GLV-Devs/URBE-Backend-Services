using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public class SocialContext : DbContext
{
    public SocialContext(DbContextOptions<SocialContext> options) : base(options)
    {
        Database.EnsureCreated();
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }

    public DbSet<SocialAppUser> Users => Set<SocialAppUser>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PendingMailConfirmation> PendingMailConfirmations => Set<PendingMailConfirmation>();

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
    }
}
