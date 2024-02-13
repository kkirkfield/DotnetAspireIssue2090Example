using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Aspire.Hosting.ApplicationModel;

public class DesignTimeDbContextFactory<TDbContext>(
    IServiceProvider serviceProvider)
    : IDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private string? _connectionString;
    private readonly TaskCompletionSource _tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);

    public TDbContext CreateDbContext()
    {
        return Task.Run(() => CreateDbContextAsync())
            .GetAwaiter()
            .GetResult();
    }

    public async Task<TDbContext> CreateDbContextAsync(
        CancellationToken cancellationToken = default)
    {
        if (_connectionString is null)
        {
            await Task.Run(() => _tcs.Task, cancellationToken);
        }

        var options = new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql(_connectionString)
            .Options;

        return ActivatorUtilities.CreateInstance<TDbContext>(_serviceProvider, options);
    }

    public void SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        _tcs.SetResult();
    }
}
