using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class CheepDTO
{
    [Required]
    public required string Author { get; set; }

    [Required]
    [StringLength(160, MinimumLength = 5)]
    public required string Text   { get; set; }

    [Required]
    public required ulong Timestamp { get; set; }
}