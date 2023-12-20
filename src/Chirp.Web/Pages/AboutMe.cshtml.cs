using Chirp.Infrastructure;
using Chirp.Web;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace;

[Authorize]
public class AboutMeModel : PageModel
{
    private readonly ICheepService service;
    private readonly SignInManager<Author> _signInManager;

    /// <summary>
    /// Stores CheepViewModels but with Timestamp as a string instead
    /// </summary>
    public required List<dynamic> Cheeps { get; set; }

    public uint CheepCount { get; private set; } = 0;
    public uint FollowerCount { get; private set; } = 0;
    public readonly HashSet<string> Followed = new();

    public AboutMeModel(
        ICheepService service,
        SignInManager<Author> signInManager)
    {
        this.service = service;
        _signInManager = signInManager;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        var cheepCount = await service.GetCheepCount(User.Identity?.Name!);
        if (cheepCount is not null)
        {
            CheepCount = (uint) cheepCount;
        }

        var followerCount = await service.GetFollowerCount(User.Identity?.Name!);
        if (followerCount is not null)
        {
            FollowerCount = (uint) followerCount;
        }

        var followed = await service.GetFollowsAll(User.Identity?.Name!);
        if (followed is not null)
        {
            foreach (var author in followed)
            {
                Followed.Add(author);
            }
        }

        return Page();
    }

    public async Task<ActionResult> OnPostUnfollowAsync(string author)
    {
        var result = await service.DeleteFollowing(User.Identity?.Name!, author);

        return await OnGetAsync();
    }
}