using System.ComponentModel.DataAnnotations;

namespace Chirp.Core;

public class CheepDTO
{
    [Required]
    public required AuthorDTO Author { get; set; }

    [Required]
    [StringLength(160, MinimumLength = 5)]
    public required string Text   { get; set; }

    [Required]
    public required DateTimeOffset Timestamp { get; set; }
}