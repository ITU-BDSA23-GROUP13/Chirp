using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheep { get; set; } = null!;
    public DbSet<Author> Author { get; set; } = null!;

    public ChirpContext(DbContextOptions options) : base(options)
    {
        Console.WriteLine("Initializing ChirpContext");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Cheep
        modelBuilder.Entity<Cheep>().HasKey(c => c.Id);
        modelBuilder.Entity<Cheep>().Property(c => c.Text).IsRequired().HasMaxLength(160);
        modelBuilder.Entity<Cheep>().Property(c => c.Timestamp).IsRequired();

        // Author : IdentityUser<string>
        modelBuilder.Entity<Author>().HasKey(a => a.Id);
        modelBuilder.Entity<Author>().Property(a => a.Id).IsRequired();//.HasConversion<ulong>();
        modelBuilder.Entity<Author>().Property(a => a.UserName).IsRequired().HasMaxLength(50);
        modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        modelBuilder.Entity<Author>().HasMany(a => a.Cheeps).WithOne(c => c.Author).IsRequired(); // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many

        // https://learn.microsoft.com/en-us/answers/questions/1242885/set-to-no-deletes-for-entity-framework-many-to-man
        modelBuilder.Entity<Author>().HasMany(a => a.Follows).WithMany(a => a.FollowedBy).UsingEntity(f => f.ToTable("Following"));

        base.OnModelCreating(modelBuilder);
    }
}