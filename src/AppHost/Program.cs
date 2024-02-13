using RazorPagesApp.Data;

var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("postgres")
    .AddDatabase("database")
    .WithDesignTimeDbContext<PostgresDatabaseResource, ApplicationDbContext>(applyMigrations: true);

builder.AddProject<Projects.RazorPagesApp>("app")
    .WithReference(database)
    .WithLaunchProfile("https");

builder.Build().Run();
