namespace Chirp.Infrastructure;

public class Cheep
{
    public required string   AuthorName { get; set; }
    public required string   Text       { get; set; }
    public required DateTime Timestamp  { get; set; }
}