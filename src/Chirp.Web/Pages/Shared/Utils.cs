
using Microsoft.AspNetCore.Identity;

namespace Chirp.Web;

public static class Utils
{
    public static async Task<User> CreateUser<User>
    (
        string name, IUserStore<User> userStore,
        string email, IUserEmailStore<User> emailStore
    )
        where User: class
    {
        User user;
        try
        {
            user = Activator.CreateInstance<User>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor");
        }

        await userStore.SetUserNameAsync(user, name, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);

        return user;
    }

    public static async Task ConfirmUser<User>(User user, UserManager<User> userManager)
        where User: class
    {
        // NOTE: This is the code that should be replaced
        // if we ever want to actually verify the email.
        var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userManager.ConfirmEmailAsync(user, code);
    }
}