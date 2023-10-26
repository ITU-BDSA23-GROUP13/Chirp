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

    /*[Required]
    public required List<CheepDTO> Cheeps { get; set; }*/
}