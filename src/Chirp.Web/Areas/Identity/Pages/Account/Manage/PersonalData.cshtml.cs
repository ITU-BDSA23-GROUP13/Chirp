// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Chirp.Infrastructure;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Web.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<Author> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        [FromQuery(Name = "show")]
        public required bool ShowData { get; set; } = false;
        public Dictionary<string, string?> Data { get; } = new();

        public PersonalDataModel(
            UserManager<Author> userManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (ShowData)
            {
                await PopulateWithPersonalData(Data, user, _userManager, _logger);
            }

            return Page();
        }

        public static async Task PopulateWithPersonalData(Dictionary<string, string?> personalData, Author author, UserManager<Author> userManager, ILogger logger)
        {
            logger.LogInformation("User with ID '{UserId}' asked for their personal data.", author.Id);

            var personalDataProps = typeof(Author).GetProperties().Where(
                prop => Attribute.IsDefined(prop, typeof(PersonalDataAttribute)));
            foreach (var p in personalDataProps)
            {
                // We don't store phone numbers.
                if (p.Name.StartsWith("Phone")) continue;

                personalData.Add(p.Name, p.GetValue(author)?.ToString());
            }

            var logins = await userManager.GetLoginsAsync(author);
            foreach (var l in logins)
            {
                personalData.Add($"{l.LoginProvider} external login provider key", l.ProviderKey);
            }

            personalData.Add($"Authenticator Key", await userManager.GetAuthenticatorKeyAsync(author));
        }
    }
}