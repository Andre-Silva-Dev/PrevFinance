using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace PrevFinance.IntegrationTests.Infrastructure;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;

    public ApiWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        builder.ConfigureAppConfiguration((_, configBuilder) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString,
                ["Seed:EnableTechnicalSeed"] = "false",
                ["Jwt:Issuer"] = "PrevFinance.Test",
                ["Jwt:Audience"] = "PrevFinance.Test.Client",
                ["Jwt:SigningKey"] = "PrevFinance-Test-Signing-Key-Needs-Min-32-Bytes-2026",
                ["Jwt:AccessTokenMinutes"] = "30",
                ["Jwt:RefreshTokenDays"] = "7"
            };

            configBuilder.AddInMemoryCollection(settings);
        });
    }
}
