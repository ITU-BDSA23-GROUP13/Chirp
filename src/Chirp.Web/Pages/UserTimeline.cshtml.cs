using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService service;
    /// <summary>
    /// Stores CheepViewModels but with Timestamp as a string instead
    /// </summary>
    public List<dynamic>? Cheeps { get; set; }

    [FromQuery(Name = "page")]
    public required uint PageNumber { get; set; }
    public required uint TotalPageCount { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGet(string author, [FromQuery(Name = "page")] uint page)
    {
        var result = await service.GetCheepsAndTotalCountFromAuthor(author, page);

        if (result is not null)
        {
            Cheeps = PublicModel.ToCheepsWithFormattedTimestamp(result.Value.Item1);
            TotalPageCount = result.Value.Item2;

            return Page();
        }

        Cheeps = null;
        TotalPageCount = 0;

        return Page();
    }

    public async Task<bool> IsFollowing(string author)
    {
        var result = await service.GetFollow(User.Identity?.Name!, author);

        if (result is null)
        {
            throw new Exception($"Could not get following between {User.Identity?.Name!} and {author}");
        }

        return (bool) result;
    }

    public async Task<ActionResult> OnPostFollow(string author)
    {
        var result = await service.PutFollower(User.Identity?.Name!, author);

        return Page();
    }

    public async Task<ActionResult> OnPostUnfollow(string author)
    {
        var result = await service.DeleteFollow(User.Identity?.Name!, author);

        return Page();
    }
}