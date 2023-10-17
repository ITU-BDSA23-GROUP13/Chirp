public record AuthorDTO(string name, string email);


// We can use the DTOs in our queries, like this:
/*
var authors = await context.Authors
    .Select(a => new AuthorDTO(a.Name, a.Email))
    .ToListAsync();
*/
