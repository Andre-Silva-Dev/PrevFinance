using FluentAssertions;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Transactions;
using PrevFinance.Domain.Users;

namespace PrevFinance.UnitTests.Domain;

public class DomainEntityValidationTests
{
    [Fact]
    public void User_Create_ShouldReturnUser_WhenEmailIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var user = User.Create(userId, "owner@prevfinance.app");

        // Assert
        user.Id.Should().Be(userId);
        user.Email.Should().Be("owner@prevfinance.app");
    }

    [Fact]
    public void User_Create_ShouldThrow_WhenEmailIsInvalid()
    {
        // Act
        var action = () => User.Create(Guid.NewGuid(), "invalid-email");

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Transaction_Create_ShouldThrow_WhenAmountIsLessOrEqualToZero()
    {
        // Arrange
        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta Principal", AccountType.Checking, 1000m);

        // Act
        var action = () => Transaction.Create(
            Guid.NewGuid(),
            account.UserId,
            account.Id,
            0m,
            DateOnly.FromDateTime(DateTime.UtcNow),
            TransactionType.Expense,
            "Pagamento");

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>();
    }
}
