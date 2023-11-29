using System.Diagnostics;
using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheep { get; set; } = null!;
    public DbSet<Author> Author { get; set; } = null!;

    public ChirpContext() : base(new DbContextOptionsBuilder().UseSqlite($"Data Source=:memory:").Options)
    {
        Console.WriteLine("Initializing ChirpContext using in memory DB");
    }

    public ChirpContext(DbContextOptions options) : base(options)
    {
        Console.WriteLine("Initializing ChirpContext");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
    }

    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cheep
        modelBuilder.Entity<Cheep>().HasKey(c => c.Id);
        modelBuilder.Entity<Cheep>().Property(c => c.Text).IsRequired().HasMaxLength(160);
        modelBuilder.Entity<Cheep>().Property(c => c.Timestamp).IsRequired();

        // Author
        modelBuilder.Entity<Author>().HasKey(a => a.Id);
        modelBuilder.Entity<Author>().Property(a => a.Id).IsRequired();//.HasConversion<ulong>();
        modelBuilder.Entity<Author>().Property(a => a.UserName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        modelBuilder.Entity<Author>().HasMany(a => a.Cheeps).WithOne(c => c.Author).IsRequired(); // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many

        await Database.MigrateAsync();

        base.OnModelCreating(modelBuilder);
    }
}