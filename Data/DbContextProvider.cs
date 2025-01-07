using Catedra3.Model;
using Microsoft.EntityFrameworkCore;

namespace Catedra3.Data;

public class DbContextProvider : DbContext
{
    
    
    public DbContextProvider(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null;

    public DbSet<Post> Posts { get; set; } = null;

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    )
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);
    }

    
}