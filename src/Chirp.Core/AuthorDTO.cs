using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class AuthorDTO
{
    [Required]
    [StringLength(50, MinimumLength = 5)]
    public required string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(160, MinimumLength = 5)]
    public required string Email { get; set; }

    /*
    [Required]
    public required List<CheepDTO> Cheeps { get; set; }

    [Required]
    public required List<AuthorDTO> Followed { get; set; }
    */

    public override bool Equals(object? other)
    {
        if (other is AuthorDTO)
        {
            var a = (AuthorDTO) other;
            return Name == a.Name && Email == a.Email;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Email);
    }

    public override string? ToString()
    {
        return $"AuthorDTO {{ Name: {Name}, Email: {Email} }}";
    }
}