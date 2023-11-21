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

    public enum Reaction
    {
        // ChatGPT helped come up with ideas for reactions fitting out theme.
        Chirp,
        Gigglegill,
        FumingFins,
        BubbleHug,
    }

    [Required]
    public required IDictionary<AuthorDTO, Reaction> Reactions { get; set; } = new Dictionary<AuthorDTO, Reaction>();
}