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
    public void User_CreateWithPassword_ShouldStorePasswordHash_WhenDataIsValid()
    {
        var user = User.CreateWithPassword(Guid.NewGuid(), "secure@prevfinance.app", "hash-value");

        user.PasswordHash.Should().Be("hash-value");
    }

    [Fact]
    public void Profile_UpdateFullName_ShouldTrimName_WhenInputIsValid()
    {
        var profile = Profile.Create(Guid.NewGuid(), Guid.NewGuid(), "User Name");

        profile.UpdateFullName("  Updated Name  ");

        profile.FullName.Should().Be("Updated Name");
    }

    [Fact]
    public void Account_Rename_ShouldTrimName_WhenInputIsValid()
    {
        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta", AccountType.Checking, 100m);

        account.Rename("  Conta Nova  ");

        account.Name.Should().Be("Conta Nova");
    }

    [Fact]
    public void Account_Create_ShouldUseInitialBalanceAsCurrentBalance_WhenCreated()
    {
        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta", AccountType.Checking, 245.7m);

        account.InitialBalance.Should().Be(245.7m);
        account.CurrentBalance.Should().Be(245.7m);
    }

    [Fact]
    public void Account_RecalibrateBalance_ShouldUpdateCurrentBalance_WhenValueChanges()
    {
        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta", AccountType.Savings, 100m);

        account.RecalibrateBalance(83.45m);

        account.CurrentBalance.Should().Be(83.45m);
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

    [Fact]
    public void InstallmentPlan_Create_ShouldThrow_WhenInstallmentCountIsInvalid()
    {
        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta", AccountType.Checking, 1000m);

        var action = () => InstallmentPlan.Create(
            Guid.NewGuid(),
            account.UserId,
            account.Id,
            1200m,
            0,
            new DateOnly(2026, 1, 31),
            InstallmentFrequency.Monthly,
            "Notebook",
            TransactionType.Expense);

        action.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void InstallmentPlan_Create_ShouldReturnEntity_WhenDataIsValid()
    {
        var account = Account.Create(Guid.NewGuid(), Guid.NewGuid(), "Conta", AccountType.Checking, 1000m);

        var plan = InstallmentPlan.Create(
            Guid.NewGuid(),
            account.UserId,
            account.Id,
            1200m,
            12,
            new DateOnly(2026, 1, 31),
            InstallmentFrequency.Monthly,
            "Notebook",
            TransactionType.Expense);

        plan.TotalAmount.Should().Be(1200m);
        plan.InstallmentCount.Should().Be(12);
        plan.Frequency.Should().Be(InstallmentFrequency.Monthly);
    }
}
