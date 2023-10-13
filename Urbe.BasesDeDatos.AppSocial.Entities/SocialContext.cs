using Microsoft.EntityFrameworkCore;
using Urbe.BasesDeDatos.AppSocial.Entities.Interfaces;
using Urbe.BasesDeDatos.AppSocial.Entities.Models;

namespace Urbe.BasesDeDatos.AppSocial.Entities;

public class SocialContext : DbContext
{
    public SocialContext(DbContextOptions<SocialContext> options) : base(options)
    {
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();

    private static void ChangeTracker_StateChanged(object? sender, Microsoft.EntityFrameworkCore.ChangeTracking.EntityStateChangedEventArgs e)
    {
        if (e.Entry.State is EntityState.Modified && e.Entry.Entity is ModifiableEntity modifiable) 
            modifiable.LastModified = DateTimeOffset.Now;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        User.BuildModel(modelBuilder, modelBuilder.Entity<User>());
        Post.BuildModel(modelBuilder, modelBuilder.Entity<Post>());
    }
}
