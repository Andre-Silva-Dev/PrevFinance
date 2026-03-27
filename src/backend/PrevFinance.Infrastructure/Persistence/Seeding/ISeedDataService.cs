namespace PrevFinance.Infrastructure.Persistence.Seeding;

public interface ISeedDataService
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}
