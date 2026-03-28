using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Transactions;

public sealed class Transaction : AuditableEntity
{
    private Transaction(
        Guid id,
        Guid userId,
        Guid accountId,
        decimal amount,
        DateOnly occurredOn,
        TransactionType type,
        string description,
        TransactionStatus status,
        Guid? installmentPlanId,
        int? installmentNumber,
        int? installmentCount) : base(id)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account id cannot be empty.", nameof(accountId));
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description is required.", nameof(description));
        }

        UserId = userId;
        AccountId = accountId;
        Amount = amount;
        OccurredOn = occurredOn;
        Type = type;
        Description = description.Trim();
        Status = status;
        InstallmentPlanId = installmentPlanId;
        InstallmentNumber = installmentNumber;
        InstallmentCount = installmentCount;
    }

    private Transaction() : base(Guid.NewGuid())
    {
        Description = string.Empty;
    }

    public Guid UserId { get; private set; }

    public Guid AccountId { get; private set; }

    public decimal Amount { get; private set; }

    public DateOnly OccurredOn { get; private set; }

    public TransactionType Type { get; private set; }

    public string Description { get; private set; }

    public TransactionStatus Status { get; private set; }

    public Guid? InstallmentPlanId { get; private set; }

    public int? InstallmentNumber { get; private set; }

    public int? InstallmentCount { get; private set; }

    public static Transaction Create(
        Guid id,
        Guid userId,
        Guid accountId,
        decimal amount,
        DateOnly occurredOn,
        TransactionType type,
        string description,
        TransactionStatus status = TransactionStatus.Pending,
        Guid? installmentPlanId = null,
        int? installmentNumber = null,
        int? installmentCount = null)
    {
        return new Transaction(
            id,
            userId,
            accountId,
            amount,
            occurredOn,
            type,
            description,
            status,
            installmentPlanId,
            installmentNumber,
            installmentCount);
    }
}
