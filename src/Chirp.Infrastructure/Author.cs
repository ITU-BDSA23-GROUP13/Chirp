namespace Chirp.Infrastructure;

public class Author
{
    public required ulong  Id    { get; set; }
    public required string Name  { get; set; }
    public required string Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
}