namespace PrevFinance.Domain.Common;

public abstract class AuditableEntity
{
    protected AuditableEntity(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Entity id cannot be empty.", nameof(id));
        }

        Id = id;
    }

    public Guid Id { get; protected set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public void MarkAsCreated(DateTime utcNow)
    {
        CreatedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public void MarkAsUpdated(DateTime utcNow)
    {
        UpdatedAtUtc = utcNow;
    }
}
