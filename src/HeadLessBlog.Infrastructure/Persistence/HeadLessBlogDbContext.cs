using HeadLessBlog.Domain.Common;
using HeadLessBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HeadLessBlog.Infrastructure.Persistence;

public class HeadLessBlogDbContext : DbContext
{
    public HeadLessBlogDbContext(DbContextOptions<HeadLessBlogDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Comment> Comments => Set<Comment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.CountryCode).IsRequired().HasMaxLength(2);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();
            entity.Property(e => e.Role)
        .IsRequired()
        .HasConversion<int>();

            entity.HasMany(e => e.Posts)
                .WithOne(p => p.User!)
                .HasForeignKey(p => p.UserId);

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.User!)
                .HasForeignKey(c => c.UserId);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();

            entity.HasMany(e => e.Comments)
                .WithOne(c => c.Post!)
                .HasForeignKey(c => c.PostId);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.IsDeleted).IsRequired();
        });
    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    foreach (var entry in ChangeTracker.Entries())
    {
        if (entry.Entity is BaseEntity baseEntity)
        {
            if (entry.State == EntityState.Added)
            {
                baseEntity.CreatedAt = DateTime.UtcNow;
                baseEntity.UpdatedAt = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                baseEntity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    return await base.SaveChangesAsync(cancellationToken);
}

}
