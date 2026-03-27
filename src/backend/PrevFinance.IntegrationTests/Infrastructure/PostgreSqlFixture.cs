using DotNet.Testcontainers.Builders;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Infrastructure.Persistence;
using Testcontainers.PostgreSql;

namespace PrevFinance.IntegrationTests.Infrastructure;

public sealed class PostgreSqlFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _container;

    public bool IsAvailable { get; private set; }

    public string? UnavailableReason { get; private set; }

    public string ConnectionString => _container?.GetConnectionString() ?? string.Empty;

    public async Task InitializeAsync()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithImage("postgres:17-alpine")
                .WithDatabase("prevfinance")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();

            await _container.StartAsync();

            await using var context = CreateDbContext();
            await context.Database.MigrateAsync();

            IsAvailable = true;
        }
        catch (Exception ex)
        {
            IsAvailable = false;
            UnavailableReason = ex.Message;
        }
    }

    public async Task DisposeAsync()
    {
        if (_container is not null)
        {
            await _container.DisposeAsync();
        }
    }

    public PrevFinanceDbContext CreateDbContext()
    {
        if (!IsAvailable || _container is null)
        {
            throw new InvalidOperationException(
                $"PostgreSQL container is unavailable. {UnavailableReason}");
        }

        var options = new DbContextOptionsBuilder<PrevFinanceDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        return new PrevFinanceDbContext(options);
    }
}
