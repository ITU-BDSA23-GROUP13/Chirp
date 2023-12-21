using Chirp.Core;
using Chirp.Infrastructure;
using Chirp.Web;

using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
}
else
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.json");
}

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<ICheepService, CheepService>();
builder.Services.AddScoped<ICheepRepository, CheepRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddDbContext<ChirpContext>(options =>
{
    try
    {
        // https://stackoverflow.com/questions/63777518/add-credentials-to-connection-string-from-code
        var connString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? throw new NullReferenceException("AZURE_SQL_CONNECTIONSTRING not set");
        var connStringBuilder = new SqlConnectionStringBuilder(connString);
        connStringBuilder.Password = Environment.GetEnvironmentVariable("CHIRP_SQL_PASSWORD") ?? throw new NullReferenceException("CHIRP_SQL_PASSWORD not set");
        options.UseSqlServer(connStringBuilder.ConnectionString);
    }
    catch(Exception e)
    {
        Console.WriteLine($"Could not create connection string for the SQL server: \"{e.Message}\". Using Sqlite database instead.");
        var path = Environment.GetEnvironmentVariable("CHIRP_LOCAL_DB") ?? Path.Join(Path.GetTempPath(), "chirp.db");
        Console.WriteLine($"Using local Sqlite database at {path}");
        options.UseSqlite($"Data Source={path}");
    }
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Author>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ChirpContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 6;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

var githubClientId = Environment.GetEnvironmentVariable("CHIRP_GITHUB_CLIENT_ID");
var githubClientSecret = Environment.GetEnvironmentVariable("CHIRP_GITHUB_CLIENT_SECRET");
if (githubClientSecret is not null)
{
    builder.Services.AddAuthentication()
        .AddGitHub(options =>
        {
            options.ClientId = githubClientId ?? "Iv1.dcb4bc25e1621f2c";
            options.ClientSecret = githubClientSecret;
        });
}
else
{
    Console.WriteLine("Could not add GitHub as authenticator. Either CHIRP_GITHUB_CLIENT_SECRET or CHIRP_GITHUB_CLIENT_ID was not set. GitHub authentication will not be available.");
}
builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseDeveloperExceptionPage();
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

using var serviceScope = app.Services.CreateScope();
var chirpContext = serviceScope.ServiceProvider.GetRequiredService<ChirpContext>();
await chirpContext.Database.EnsureCreatedAsync();
DBInitializer.SeedDatabase(chirpContext);

app.MapRazorPages();

app.Run();

public partial class Program {}