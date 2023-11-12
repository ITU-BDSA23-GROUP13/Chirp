using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService service;
    public required List<CheepViewModel> Cheeps { get; set; }
    public required uint TotalCount { get; set; }

    public PublicModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGet([FromQuery(Name = "page")] uint page = 1)
    {
        var result = await service.GetCheepsAndTotalCount(page);

        Cheeps = result.Item1;
        TotalCount = result.Item2;

        return Page();
    }
}