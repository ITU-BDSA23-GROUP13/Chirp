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

    [FromQuery(Name = "page")]
    public required uint PageNumber { get; set; }
    public required uint TotalPageCount { get; set; }

    public FollowedModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery(Name = "page")] uint page)
    {
        var result = await service.GetCheepsAndTotalCountFromFollowed(User.Identity?.Name!, page);

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
}