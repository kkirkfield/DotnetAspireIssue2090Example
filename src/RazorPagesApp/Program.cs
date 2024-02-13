using Microsoft.EntityFrameworkCore;
using RazorPagesApp.Data;
using RazorPagesApp.Data.Fakers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<ApplicationDbContext>("database",
    configureDbContextOptions: optionsBuilder =>
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    });

builder.Services.AddSingleton<TodoItemFaker>();

builder.Services.AddRazorPages();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

await ApplicationDbContextInitializer.InitializeDatabaseAsync(app.Services);

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
