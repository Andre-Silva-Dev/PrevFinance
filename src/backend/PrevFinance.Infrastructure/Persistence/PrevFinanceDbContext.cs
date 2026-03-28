using Microsoft.EntityFrameworkCore;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Common;
using PrevFinance.Domain.Transactions;
using PrevFinance.Domain.Users;

namespace PrevFinance.Infrastructure.Persistence;

public sealed class PrevFinanceDbContext : DbContext
{
    public PrevFinanceDbContext(DbContextOptions<PrevFinanceDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PrevFinanceDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyAuditData();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditData();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditData()
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.MarkAsCreated(utcNow);
                continue;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.MarkAsUpdated(utcNow);
            }
        }
    }
}
