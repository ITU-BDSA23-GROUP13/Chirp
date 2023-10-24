using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGet()
    {
        Cheeps = await service.GetCheeps();
        return Page();
    }
}