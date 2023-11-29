using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

[AllowAnonymous]
public class PublicModel : PageModel
{
    private readonly ICheepService service;
    public required List<CheepViewModel> Cheeps { get; set; }

    [FromQuery(Name = "page")]
    public required uint PageNumber { get; set; }
    public required uint TotalPageCount { get; set; }

    public PublicModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        var result = await service.GetCheepsAndPageCount(PageNumber);

        Cheeps = result.Item1;
        TotalPageCount = result.Item2;

        return Page();
    }

    [BindProperty]
    public CheepViewModel? CheepViewModel { get; set; }

    public Task<ActionResult> OnPostAsync()
    {
        throw new NotImplementedException();
    }
}