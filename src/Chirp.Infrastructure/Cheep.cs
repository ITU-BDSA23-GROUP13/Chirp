namespace Chirp.Infrastructure;

public class Cheep
{
    public required ulong  Id        { get; set; }
    public required ulong  AuthorId  { get; set; }
    public required Author Author    { get; set; }
    public required string Text      { get; set; }
    public required long   Timestamp { get; set; }
}