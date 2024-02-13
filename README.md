# DotnetAspireIssue2090Example

This is a sample project to provide a proof of concept for the following issue: [https://github.com/dotnet/aspire/issues/2090](https://github.com/dotnet/aspire/issues/2090)

### Issue 1

The following scripts will fail because the EF Tools currently stop the AppHost project and its database container before the EF Tools have a chance to connect to the database.

```pwsh
dotnet ef migrations add InitialMigration `
    -s .\src\AppHost\AppHost.csproj `
    -p .\src\RazorPagesApp\RazorPagesApp.csproj `
    -v
```

```pwsh
dotnet ef migrations remove `
    -s .\src\AppHost\AppHost.csproj `
    -p .\src\RazorPagesApp\RazorPagesApp.csproj `
    -v
```

A workaround for the time being is to use the following scripts.

```pwsh
dotnet ef migrations add InitialMigration `
    -s .\src\RazorPagesApp\RazorPagesApp.csproj `
    -p .\src\RazorPagesApp\RazorPagesApp.csproj `
    -v
```

The workaround to remove migrations is below. This forces the EF Tools to remove the most recent migration from the snapshot without being able to connect to the database.

```pwsh
dotnet ef migrations remove `
    -s .\src\RazorPagesApp\RazorPagesApp.csproj `
    -p .\src\RazorPagesApp\RazorPagesApp.csproj `
    -f `
    -v
```

### Issue 2

We are also unable to apply migrations from the AppHost project because the `AfterEndpointsAllocatedAsync` lifecycle hook is called before the database container is started but after the endpoint port is assigned. This code has been commented out in `ResourceBuilderExtensions`. We need a lifecycle hook that is called after all resources are running to be able to apply migrations from the AppHost project at startup.
