using System.Runtime.InteropServices;

using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpContext : DbContext
{
    public DbSet<Cheep> Cheep { get; set; }
    public DbSet<Author> Author { get; set; }

    public string DBPath { get; }

    public ChirpContext() : base()
    {
        DBPath = Environment.GetEnvironmentVariable("CHIRP_DB") ?? Path.Join(GetDefaultDBPath(), "chirp.db");
    }

    private string GetDefaultDBPath()
    {
        string path;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            path = Path.GetTempPath();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            path = Path.GetTempPath();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            path = "/tmp";
        else
            throw new Exception("OS not supported");

        return path;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

        if (File.Exists(DBPath)) 
        {
            Console.WriteLine($"Found Sqlite DB at {DBPath}");
        }
        else
        {
            Console.WriteLine($"Could not find Sqlite DB at {DBPath}... Trying to create a new one.");
            var dir = Path.GetDirectoryName(DBPath);
            if (dir is null)
            {
                throw new Exception("Could not get directory name from DBPath. Directory was either ROOT or null.");
            }
            Directory.CreateDirectory(dir);
            File.Create(DBPath).Close();
        }

        options.UseSqlite($"Data Source={DBPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Database.MigrateAsync();

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