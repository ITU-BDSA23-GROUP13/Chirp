using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

[AllowAnonymous]
public class UserTimelineModel : PageModel
{
    private readonly ICheepService service;

    /// <summary>
    /// Stores CheepViewModels but with Timestamp as a string instead
    /// </summary>
    public List<dynamic>? Cheeps { get; set; }
    public uint FollowerCount { get; private set; } = 0;
    private readonly HashSet<string> followed = new();

    [FromQuery(Name = "page")]
    public required uint PageNumber { get; set; } = 1;
    public required uint TotalPageCount { get; set; }

    public UserTimelineModel(ICheepService service)
    {
        this.service = service;
    }

    public async Task<ActionResult> OnGetAsync(string author)
    {
        var result = await service.GetCheepsAndTotalCountFromAuthor(author, PageNumber != 0 ? PageNumber : 1);
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

        var followers = await service.GetFollowerCount(author);
        if (followers is not null)
        {
            FollowerCount = (uint) followers;
        }

        var followed = await service.GetFollowed(User.Identity?.Name!);
        if (followed is not null)
        {
            foreach (var f in followed)
            {
                this.followed.Add(f);
            }
        }

        return Page();
    }

    public bool IsFollowing(string author)
    {
        return followed.Contains(author);
    }

    public async Task<ActionResult> OnPostFollowAsync(string author)
    {
        var result = await service.PutFollower(User.Identity?.Name!, author);

        return await OnGetAsync(author);
    }

    public async Task<ActionResult> OnPostUnfollowAsync(string author)
    {
        var result = await service.DeleteFollow(User.Identity?.Name!, author);

        return await OnGetAsync(author);
    }

}