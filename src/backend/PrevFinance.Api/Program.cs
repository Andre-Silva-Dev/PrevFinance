using Microsoft.EntityFrameworkCore;
using PrevFinance.Application.DependencyInjection;
using PrevFinance.Application.Modules;
using PrevFinance.Infrastructure.DependencyInjection;
using PrevFinance.Infrastructure.Persistence;
using PrevFinance.Infrastructure.Persistence.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services
    .AddPrevFinanceApplication()
    .AddPrevFinanceInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PrevFinanceDbContext>();
    await dbContext.Database.MigrateAsync();

    if (app.Configuration.GetValue<bool>("Seed:EnableTechnicalSeed"))
    {
        var seedDataService = scope.ServiceProvider.GetRequiredService<ISeedDataService>();
        await seedDataService.SeedAsync();
    }
}

app.MapGet("/", () =>
{
    var modules = ModuleRegistry.All.Select(module => module.Name).ToArray();
    return Results.Ok(new { service = "PrevFinance.Api", modules });
});

app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

app.Run();

public partial class Program;
