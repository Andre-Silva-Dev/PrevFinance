using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Accounts;

public sealed class AccountBalanceAdjustment : AuditableEntity
{
    private AccountBalanceAdjustment(
        Guid id,
        Guid userId,
        Guid accountId,
        decimal previousBalance,
        decimal newBalance,
        string reason,
        DateTime adjustedAtUtc) : base(id)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account id cannot be empty.", nameof(accountId));
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Adjustment reason is required.", nameof(reason));
        }

        UserId = userId;
        AccountId = accountId;
        PreviousBalance = previousBalance;
        NewBalance = newBalance;
        Reason = reason.Trim();
        AdjustedAtUtc = adjustedAtUtc;
    }

    private AccountBalanceAdjustment() : base(Guid.NewGuid())
    {
        Reason = string.Empty;
    }

    public Guid UserId { get; private set; }

    public Guid AccountId { get; private set; }

    public decimal PreviousBalance { get; private set; }

    public decimal NewBalance { get; private set; }

    public string Reason { get; private set; }

    public DateTime AdjustedAtUtc { get; private set; }

    public static AccountBalanceAdjustment Create(
        Guid id,
        Guid userId,
        Guid accountId,
        decimal previousBalance,
        decimal newBalance,
        string reason,
        DateTime adjustedAtUtc)
    {
        return new AccountBalanceAdjustment(id, userId, accountId, previousBalance, newBalance, reason, adjustedAtUtc);
    }
}
