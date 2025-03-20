using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options) 
{
  public DbSet<AppUser> Users { get; set; }
  public DbSet<UserLIke> Likes { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);
    builder.Entity<UserLIke>() 
            .HasKey(k => new{k.SourceUserId, k.TargetUserId});
    
    builder.Entity<UserLIke>()
           .HasOne(s => s.SourceUser)
           .WithMany( l => l.LikeUsers )
           .HasForeignKey(s => s.SourceUserId)
           .OnDelete(DeleteBehavior.Cascade);

    builder.Entity<UserLIke>()
           .HasOne(s => s.TargetUser)
           .WithMany( l => l.LikedByUsers )
           .HasForeignKey(s => s.TargetUserId)
           .OnDelete(DeleteBehavior.NoAction);           
  }
}