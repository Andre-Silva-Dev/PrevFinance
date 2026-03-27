using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Transactions;
using PrevFinance.Domain.Users;

namespace PrevFinance.IntegrationTests.Infrastructure;

public class PersistenceIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public PersistenceIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task ShouldPersistAndQueryEntitiesByUserAndPeriod_WhenDataIsValid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        // Arrange
        await using var context = _fixture.CreateDbContext();

        var user = User.Create(Guid.NewGuid(), "user01@prevfinance.app");
        var profile = Profile.Create(Guid.NewGuid(), user.Id, "User 01");
        var account = Account.Create(Guid.NewGuid(), user.Id, "Conta Corrente", AccountType.Checking, 1200m);

        var marchExpense = Transaction.Create(
            Guid.NewGuid(),
            user.Id,
            account.Id,
            250m,
            new DateOnly(2026, 3, 10),
            TransactionType.Expense,
            "Mercado");

        context.Users.Add(user);
        context.Profiles.Add(profile);
        context.Accounts.Add(account);
        context.Transactions.Add(marchExpense);
        await context.SaveChangesAsync();

        // Act
        var start = new DateOnly(2026, 3, 1);
        var end = new DateOnly(2026, 3, 31);

        var transactions = await context.Transactions
            .Where(t => t.UserId == user.Id && t.OccurredOn >= start && t.OccurredOn <= end)
            .ToListAsync();

        // Assert
        transactions.Should().HaveCount(1);
        transactions.Single().Description.Should().Be("Mercado");
    }

    [Fact]
    public async Task ShouldFailToPersistAccount_WhenUserDoesNotExist()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        // Arrange
        await using var context = _fixture.CreateDbContext();

        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta Orfa", AccountType.Checking, 100m);
        context.Accounts.Add(account);

        // Act
        var action = async () => await context.SaveChangesAsync();

        // Assert
        var exception = await action.Should().ThrowAsync<DbUpdateException>();
        exception.Which.InnerException.Should().BeOfType<PostgresException>();
    }
}
