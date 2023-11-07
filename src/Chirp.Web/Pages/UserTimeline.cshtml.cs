using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService service;
    public required List<CheepViewModel> Cheeps { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGet(string author, [FromQuery(Name = "page")] uint page)
    {
        Cheeps = await service.GetCheepsFromAuthor(author, page);
        return Page();
    }
}