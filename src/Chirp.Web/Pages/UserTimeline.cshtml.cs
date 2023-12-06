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
    public uint? TotalCount { get; set; }

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
            TotalCount = result.Value.Item2;

            return Page();
        }

        Cheeps = null;
        TotalCount = null;

        return Page();
    }
}