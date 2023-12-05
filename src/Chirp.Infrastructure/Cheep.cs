namespace Chirp.Infrastructure;

public class Cheep
{
    public string Id = Guid.NewGuid().ToString();
    public required Author Author    { get; set; }
    public required string Text      { get; set; }
    public required long   Timestamp { get; set; }
}