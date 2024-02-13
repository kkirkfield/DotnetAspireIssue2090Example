using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Data.Fakers;

namespace RazorPagesApp.Data;

internal static class ApplicationDbContextInitializer
{
    public static async Task InitializeDatabaseAsync(
        IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        var scope = services.CreateAsyncScope();

        await using (scope.ConfigureAwait(false))
        {
            var logger = scope.ServiceProvider.GetService<ILoggerFactory>()
                ?.CreateLogger(typeof(ApplicationDbContextInitializer));

            // Wait for the database to start.
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken).ConfigureAwait(false);

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            logger?.LogInformation("Applying migrations...");

            await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

            logger?.LogInformation("Seeding database... {Progress}%", 0);

            var todoItemFaker = scope.ServiceProvider.GetRequiredService<TodoItemFaker>();

            for (int i = 0; i < 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var todoItems = todoItemFaker.GenerateLazy(100);

                await dbContext.TodoItems
                    .AddRangeAsync(todoItems, cancellationToken)
                    .ConfigureAwait(false);

                await dbContext.SaveChangesAsync(cancellationToken)
                    .ConfigureAwait(false);

                logger?.LogInformation("Seeding database... {Progress}%", (i + 1) * 10);
            }

            logger?.LogInformation("Database successfully initialized.");
        }
    }
}
