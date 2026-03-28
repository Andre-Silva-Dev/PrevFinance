using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrevFinance.Api.Auth;
using PrevFinance.Application.Abstractions;
using PrevFinance.Application.DependencyInjection;
using PrevFinance.Application.Modules;
using PrevFinance.Infrastructure.DependencyInjection;
using PrevFinance.Infrastructure.Persistence;
using PrevFinance.Infrastructure.Persistence.Seeding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

builder.Services
    .AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(options =>
        !string.IsNullOrWhiteSpace(options.Issuer) &&
        !string.IsNullOrWhiteSpace(options.Audience) &&
        !string.IsNullOrWhiteSpace(options.SigningKey),
        "Jwt settings are invalid.")
    .ValidateOnStart();

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapHealthChecks("/health");
app.MapHealthChecks("/health/ready");

app.Run();

public partial class Program;
