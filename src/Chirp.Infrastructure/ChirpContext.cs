using System.Runtime.InteropServices;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

public class ChirpContext : IdentityDbContext<Author>
{
    public DbSet<Cheep> Cheep { get; set; } = null!;
    public DbSet<Author> Author { get; set; } = null!;

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

        base.OnConfiguring(options);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Database.MigrateAsync();

        // Cheep
        //modelBuilder.Entity<Cheep>().HasKey(c => c.Id);
        //modelBuilder.Entity<Cheep>().Property(c => c.Text).IsRequired().HasMaxLength(160);
        //modelBuilder.Entity<Cheep>().Property(c => c.Timestamp).IsRequired();

        // Author
        //modelBuilder.Entity<Author>().HasKey(a => a.Id);
        //modelBuilder.Entity<Author>().Property(a => a.Id).IsRequired();//.HasConversion<ulong>();
        //modelBuilder.Entity<Author>().Property(a => a.UserName).IsRequired().HasMaxLength(50);
        //modelBuilder.Entity<Author>().Property(a => a.Email).IsRequired();
        //modelBuilder.Entity<Author>().HasMany(a => a.Cheeps).WithOne(c => c.Author).IsRequired(); // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many       

        base.OnModelCreating(modelBuilder);
    }
}