using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

using Chirp.Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Pages;

[AllowAnonymous]
public class PublicModel : PageModel
{
    private readonly ICheepService service;
    private readonly SignInManager<Author> _signInManager;
    /// <summary>
    /// Stores CheepViewModels but with Timestamp as a string instead
    /// </summary>
    public required List<dynamic> Cheeps { get; set; }
    private readonly HashSet<string> followed = new();

    [FromQuery(Name = "page")]
    public required uint PageNumber { get; set; }
    public required uint TotalPageCount { get; set; }

    public PublicModel(
        ICheepService service,
        SignInManager<Author> signInManager)
    {
        this.service = service;
        _signInManager = signInManager;
    }

    public async Task<ActionResult> OnGetAsync()
    {
        var (cheeps, count) = await service.GetCheepsAndPageCount(PageNumber);

        Cheeps = ToCheepsWithFormattedTimestamp(cheeps);
        TotalPageCount = count;

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

    public static List<dynamic> ToCheepsWithFormattedTimestamp(List<CheepViewModel> cheeps)
    {
        var Cheeps = new List<dynamic>();

        var now = DateTimeOffset.Now;

        foreach (var cheep in cheeps)
        {
            // List of DateTime format characters
            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings
            // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings
            string timestamp;

            var timeSince = now.Subtract(cheep.Timestamp);

            if (cheep.Timestamp.Year != now.Year)
            {
                // Mon, June 15 '09, 1:45 PM
                timestamp = cheep.Timestamp.ToString(@"ddd, MMMM d 'yy, ") + cheep.Timestamp.ToString("t");
            }
            else if (cheep.Timestamp.Month != now.Month || cheep.Timestamp.Day != now.Day)
            {
                // Mon, June 15, 1:45 PM
                timestamp = cheep.Timestamp.ToString(@"ddd, MMMM d, ") + cheep.Timestamp.ToString("t");
            }
            else if(timeSince.Hours >= 1)
            {
                // today at 1:45 PM
                timestamp = "today at " + cheep.Timestamp.ToString("t");
            }
            else if(timeSince.Minutes >= 1)
            {
                // 45 minutes ago
                timestamp = timeSince.Minutes + " minutes ago";
            }
            else
            {
                timestamp = "now";
            }

            Cheeps.Add(new { Author = cheep.Author, Message = cheep.Message, Timestamp = timestamp });
        }

        return Cheeps;
    }

    [BindProperty]
    [StringLength(160, MinimumLength = 5)]
    public string Message { get; set; } = null!;

    public static string MessagePlaceholder()
    {
        return (new Random().Next() % 6) switch
        {
            // Copilot helped come up with these :)
            0 => "Write down what's on your mind, and share it with the world!",
            1 => "What's happening?",
            2 => "Please don't say anything mean.",
            3 => "Don't be shy!",
            4 => "What's up?",
            5 => "Enter something here to post a cheep!",
            _ => throw new UnreachableException(),
        };
    }

    public async Task<ActionResult> OnPostCheepAsync()
    {
        if (!_signInManager.IsSignedIn(User))
        {
            return Page();
        }

        var cheep = new CheepViewModel
        {
            Author = User.Identity?.Name!,
            Message = Message,
            Timestamp = DateTimeOffset.Now,
        };

        await service.PutCheep(cheep);

        return Redirect($"~/{cheep.Author}");
    }

    public async Task<ActionResult> OnPostFollowAsync(string author)
    {
        var result = await service.PutFollower(User.Identity?.Name!, author);

        return await OnGetAsync();
    }

    public async Task<ActionResult> OnPostUnfollowAsync(string author)
    {
        var result = await service.DeleteFollow(User.Identity?.Name!, author);

        return await OnGetAsync();
    }

}