using Microsoft.EntityFrameworkCore;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Users;

namespace PrevFinance.Infrastructure.Persistence.Seeding;

public sealed class TechnicalSeedDataService : ISeedDataService
{
    private readonly PrevFinanceDbContext _context;

    public TechnicalSeedDataService(PrevFinanceDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Users.AnyAsync(cancellationToken))
        {
            return;
        }

        var user = User.Create(Guid.Parse("11111111-1111-1111-1111-111111111111"), "seed@prevfinance.app");
        var profile = Profile.Create(Guid.Parse("22222222-2222-2222-2222-222222222222"), user.Id, "Technical Seed User");
        var account = Account.Create(Guid.Parse("33333333-3333-3333-3333-333333333333"), user.Id, "Conta Seed", AccountType.Checking, 1000m);

        _context.Users.Add(user);
        _context.Profiles.Add(profile);
        _context.Accounts.Add(account);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
