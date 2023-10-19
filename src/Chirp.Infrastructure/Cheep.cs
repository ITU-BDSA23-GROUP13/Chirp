namespace Chirp.Infrastructure;

public class Cheep
{
    public required string Id        { get; set; }
    public required string AuthorId  { get; set; }
    public required string Text      { get; set; }
    public required long   Timestamp { get; set; }
}