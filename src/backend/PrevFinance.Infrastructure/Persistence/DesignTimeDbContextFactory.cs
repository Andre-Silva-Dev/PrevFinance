using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PrevFinance.Infrastructure.Persistence;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PrevFinanceDbContext>
{
    public PrevFinanceDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("PREVFINANCE_DB_CONNECTION")
            ?? "Host=localhost;Port=5432;Database=prevfinance;Username=postgres;Password=postgres";

        var options = new DbContextOptionsBuilder<PrevFinanceDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new PrevFinanceDbContext(options);
    }
}
