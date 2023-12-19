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
    // https://stackoverflow.com/questions/63777518/add-credentials-to-connection-string-from-code
    var connString = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING") ?? throw new NullReferenceException("AZURE_SQL_CONNECTIONSTRING not set");
    var connStringBuilder = new SqlConnectionStringBuilder(connString);
    connStringBuilder.Password = Environment.GetEnvironmentVariable("CHIRP_SQL_PASSWORD") ?? throw new NullReferenceException("CHIRP_SQL_PASSWORD not set");
    options.UseSqlServer(connStringBuilder.ConnectionString);
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
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.AddAuthentication()
    .AddGitHub(options =>
    {
        options.ClientId = "Iv1.dcb4bc25e1621f2c";
        options.ClientSecret = Environment.GetEnvironmentVariable("CHIRP_GITHUB_CLIENT_SECRET") ?? throw new NullReferenceException("CHIRP_GITHUB_CLIENT_SECRET not set");
    });
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