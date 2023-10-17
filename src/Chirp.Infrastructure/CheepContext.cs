using Chirp.Core;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class CheepContext : DbContext
{
    public DbSet<CheepDTO>? Cheeps { get; set; }
    public DbSet<AuthorDTO>? Authors { get; set; }

    public string DBPath { get; }

    public CheepContext()
    {
        var path = Environment.GetEnvironmentVariable("CHIRP_DB") ?? Path.GetTempPath();
        DBPath = Path.Join(path, "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DBPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheepDTO>().Property(c => c.Author).IsRequired();

        modelBuilder.Entity<CheepDTO>().Property(c => c.Text).IsRequired();
        modelBuilder.Entity<CheepDTO>().Property(c => c.Text).HasMaxLength(160);

        modelBuilder.Entity<CheepDTO>().Property(c => c.Timestamp).IsRequired();

        modelBuilder.Entity<AuthorDTO>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<AuthorDTO>().Property(a => a.Name).IsRequired();
        modelBuilder.Entity<AuthorDTO>().Property(a => a.Name).HasMaxLength(50);

        modelBuilder.Entity<AuthorDTO>().Property(a => a.Email).IsRequired();

        modelBuilder.Entity<AuthorDTO>().Property(a => a.Cheeps).IsRequired();
    }
}