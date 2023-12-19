using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

public class FollowedModel : PageModel
{
    private readonly ICheepService service;
    /// <summary>
    /// Stores CheepViewModels but with Timestamp as a string instead
    /// </summary>
    public List<dynamic>? Cheeps { get; set; }
    private readonly HashSet<string> followed = new();

    [FromQuery(Name = "page")]
    public required uint PageNumber { get; set; } = 1;
    public required uint TotalPageCount { get; set; }

    public FollowedModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        var result = await service.GetCheepsAndTotalCountFromFollowed(User.Identity?.Name!, PageNumber != 0 ? PageNumber : 1);
        if (result is not null)
        {
            Cheeps = PublicModel.ToCheepsWithFormattedTimestamp(result.Value.Item1);
            TotalPageCount = result.Value.Item2;
        }
        else
        {
            Cheeps = null;
            TotalPageCount = 0;
        }

        var followed = await service.GetFollowed(User.Identity?.Name!);
        if (followed is not null)
        {
            foreach (var author in followed)
            {
                this.followed.Add(author);
            }
        }

        return Page();
    }

    public bool IsFollowing(string author)
    {
        return followed.Contains(author);
    }

    // A OnPostFollow should be unnecessary since this page only shows authors that are already followed.
    
    public async Task<ActionResult> OnPostUnfollowAsync(string author)
    {
        var result = await service.DeleteFollow(User.Identity?.Name!, author);

        return await OnGetAsync();
    }
}