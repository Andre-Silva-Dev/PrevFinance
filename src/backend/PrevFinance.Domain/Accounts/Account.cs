using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Accounts;

public sealed class Account : AuditableEntity
{
    private Account(Guid id, Guid userId, string name, AccountType type, decimal initialBalance) : base(id)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Account name is required.", nameof(name));
        }

        UserId = userId;
        Name = name.Trim();
        Type = type;
        InitialBalance = initialBalance;
        CurrentBalance = initialBalance;
    }

    private Account() : base(Guid.NewGuid())
    {
        Name = string.Empty;
    }

    public Guid UserId { get; private set; }

    public string Name { get; private set; }

    public AccountType Type { get; private set; }

    public decimal InitialBalance { get; private set; }

    public decimal CurrentBalance { get; private set; }

    public static Account Create(Guid id, Guid userId, string name, AccountType type, decimal initialBalance)
    {
        return new Account(id, userId, name, type, initialBalance);
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Account name is required.", nameof(name));
        }

        Name = name.Trim();
    }

    public decimal RecalibrateBalance(decimal newBalance)
    {
        var previousBalance = CurrentBalance;
        CurrentBalance = newBalance;
        return previousBalance;
    }
}
