using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Users;

public sealed class Profile : AuditableEntity
{
    private Profile(Guid id, Guid userId, string fullName) : base(id)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        UserId = userId;
        FullName = fullName;
    }

    private Profile() : base(Guid.NewGuid())
    {
        FullName = string.Empty;
    }

    public Guid UserId { get; private set; }

    public string FullName { get; private set; }

    public static Profile Create(Guid id, Guid userId, string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
        {
            throw new ArgumentException("Full name is required.", nameof(fullName));
        }

        return new Profile(id, userId, fullName.Trim());
    }
}
