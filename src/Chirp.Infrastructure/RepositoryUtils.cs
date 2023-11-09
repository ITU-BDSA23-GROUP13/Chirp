
using Microsoft.EntityFrameworkCore;

namespace Chirp.Infrastructure;

internal static class RepositoryUtils
{
    // Async/await is unnesecary if you don't want to be able to return back to the function after an await point. Here we simply would simply return immediately after awaiting. https://stackoverflow.com/questions/38017016/async-task-then-await-task-vs-task-then-return-task
    // Cannot return null instead of generic type because valuetypes/structs aren't stored as references. https://stackoverflow.com/questions/302096/how-can-i-return-null-from-a-generic-method-in-c
    internal static Task<T> TryGetFirstAsync<T>(IQueryable<T> query, T def)
    {
        try
        {
            return query.FirstAsync();
        }
        catch (InvalidOperationException)
        {
            return Task.FromResult(def);
        }
    }

    internal static Task<T?> TryGetFirstAsyncElseNull<T>(IQueryable<T> query) where T: class
        => TryGetFirstAsync<T?>(query, null);

}