using Microsoft.AspNetCore.Identity;

namespace Chirp.Infrastructure;

public class Author : IdentityUser
{
    //public required ulong  Id    { get; set; }
    //public required string Name  { get; set; }
    //public required string Email { get; set; }
    public List<Cheep>  Cheeps    { get; set; } = new();
    public List<Author> Followed  { get; set; } = new();
    public List<Author> Followers { get; set; } = new();

    public Author()
    {
        Id = new Guid().ToString();
    }
}