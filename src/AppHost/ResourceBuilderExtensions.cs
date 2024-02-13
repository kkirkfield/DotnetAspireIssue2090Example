using Aspire.Hosting.Lifecycle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire.Hosting.ApplicationModel;

public static class ResourceBuilderExtensions
{
    public static IResourceBuilder<TResource> WithDesignTimeDbContext<TResource, TDbContext>(
        this IResourceBuilder<TResource> builder,
        bool applyMigrations = false)
        where TResource : IResourceWithConnectionString
        where TDbContext : DbContext
    {
        var resourceName = builder.Resource.Name;

        builder.ApplicationBuilder.Services.AddDbContextFactory<TDbContext, DesignTimeDbContextFactory<TDbContext>>();

        builder.ApplicationBuilder.Services.AddLifecycleHook(services =>
        {
            var dbContextFactory = (DesignTimeDbContextFactory<TDbContext>)services.GetRequiredService<IDbContextFactory<TDbContext>>();

            return new ConnectionStringAvailableLifecycleHook(resourceName, async (connectionString, cancellationToken) =>
            {
                dbContextFactory.SetConnectionString(connectionString);

                if (applyMigrations)
                {
                    //using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);

                    //await dbContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
                }
            });
        });

        return builder;
    }
}
