using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    public string DBPath { get; }

    public ChirpContext() : base()
    {
        var path = Environment.GetEnvironmentVariable("CHIRP_DB") ?? Path.GetTempPath();
        DBPath = Path.Join(path, "chirp.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DBPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cheep
        modelBuilder.Entity<Cheep>().HasKey(c => c.Id);
        modelBuilder.Entity<Cheep>().Property(c => c.AuthorId).IsRequired();
        modelBuilder.Entity<Cheep>().Property(c => c.Text).IsRequired().HasMaxLength(160);
        modelBuilder.Entity<Cheep>().Property(c => c.Timestamp).IsRequired();

        // Author
        modelBuilder.Entity<Author>().HasKey(a => a.Id);
        modelBuilder.Entity<Author>().HasIndex(a => a.Name).IsUnique();
        modelBuilder.Entity<Author>().Property(a => a.Name).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Author>().HasIndex(a => a.Email).IsUnique();
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
    }
}