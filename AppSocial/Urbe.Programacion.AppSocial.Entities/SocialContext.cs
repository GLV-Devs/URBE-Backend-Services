using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Urbe.Programacion.AppSocial.Entities.Models;
using Urbe.Programacion.Shared.Entities;

namespace Urbe.Programacion.AppSocial.Entities;

public class SocialContext : BaseAppContext
{
    public SocialContext(DbContextOptions<SocialContext> options) : base(options) { }
    public DbSet<SocialAppUser> SocialAppUsers => Set<SocialAppUser>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PendingMailConfirmation> PendingMailConfirmations => Set<PendingMailConfirmation>();
    public DbSet<SocialAppUserFollow> SocialAppUserFollows => Set<SocialAppUserFollow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SocialAppUser.BuildModel(modelBuilder, modelBuilder.Entity<SocialAppUser>(), this);
        PendingMailConfirmation.BuildModel(modelBuilder, modelBuilder.Entity<PendingMailConfirmation>(), this);
        Post.BuildModel(modelBuilder, modelBuilder.Entity<Post>(), this);

        var iucmb = modelBuilder.Entity<IdentityUserClaim<Guid>>();
        iucmb.HasKey(x => x.Id);
        iucmb.HasOne(typeof(SocialAppUser)).WithMany().HasForeignKey(nameof(IdentityUserClaim<Guid>.UserId));
    }
}
