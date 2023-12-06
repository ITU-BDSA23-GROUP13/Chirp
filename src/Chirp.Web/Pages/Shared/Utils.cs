
using Chirp.Infrastructure;

using Microsoft.AspNetCore.Identity;

namespace Chirp.Web;

public static class Utils
{
    public static async Task<Author> CreateUser
    (
        string name, IUserStore<Author> userStore,
        string email, IUserEmailStore<Author> emailStore
    )
    {
        Author user;
        try
        {
            user = Activator.CreateInstance<Author>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(Author)}'. " +
                $"Ensure that '{nameof(Author)}' is not an abstract class and has a parameterless constructor");
        }

        await userStore.SetUserNameAsync(user, name, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);

        return user;
    }

    public static async Task ConfirmUser(Author user, UserManager<Author> userManager)
    {
        // NOTE: This is the code that should be replaced
        // if we ever want to actually verify the email.
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.ConfirmEmailAsync(user, code);
    }
}