using Aspire.Hosting.Lifecycle;

namespace Aspire.Hosting.ApplicationModel;

public class ConnectionStringAvailableLifecycleHook(
    string resourceName,
    Func<string, CancellationToken, Task> connectionStringAvailable)
    : IDistributedApplicationLifecycleHook
{
    private readonly string _resourceName = resourceName;
    private readonly Func<string, CancellationToken, Task> _connectionStringAvailable = connectionStringAvailable;

    public async Task AfterEndpointsAllocatedAsync(
        DistributedApplicationModel appModel,
        CancellationToken cancellationToken = default)
    {
        var resource = appModel.Resources
            .OfType<IResourceWithConnectionString>()
            .FirstOrDefault(r => r.Name == _resourceName);

        if (resource is null)
        {
            return;
        }

        var connectionString = resource.GetConnectionString();

        if (connectionString is not null)
        {
            await _connectionStringAvailable.Invoke(connectionString, cancellationToken).ConfigureAwait(false);
        }
    }
}
