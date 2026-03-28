using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Transactions;

public sealed class InstallmentPlan : AuditableEntity
{
    private InstallmentPlan(
        Guid id,
        Guid userId,
        Guid accountId,
        decimal totalAmount,
        int installmentCount,
        DateOnly startDate,
        InstallmentFrequency frequency,
        string description,
        TransactionType type) : base(id)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        if (accountId == Guid.Empty)
        {
            throw new ArgumentException("Account id cannot be empty.", nameof(accountId));
        }

        if (totalAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount must be greater than zero.");
        }

        if (installmentCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(installmentCount), "Installment count must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description is required.", nameof(description));
        }

        UserId = userId;
        AccountId = accountId;
        TotalAmount = totalAmount;
        InstallmentCount = installmentCount;
        StartDate = startDate;
        Frequency = frequency;
        Description = description.Trim();
        Type = type;
    }

    private InstallmentPlan() : base(Guid.NewGuid())
    {
        Description = string.Empty;
    }

    public Guid UserId { get; private set; }

    public Guid AccountId { get; private set; }

    public decimal TotalAmount { get; private set; }

    public int InstallmentCount { get; private set; }

    public DateOnly StartDate { get; private set; }

    public InstallmentFrequency Frequency { get; private set; }

    public string Description { get; private set; }

    public TransactionType Type { get; private set; }

    public static InstallmentPlan Create(
        Guid id,
        Guid userId,
        Guid accountId,
        decimal totalAmount,
        int installmentCount,
        DateOnly startDate,
        InstallmentFrequency frequency,
        string description,
        TransactionType type)
    {
        return new InstallmentPlan(id, userId, accountId, totalAmount, installmentCount, startDate, frequency, description, type);
    }
}
