using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Users;

public sealed class RefreshToken : AuditableEntity
{
    private RefreshToken(Guid id, Guid userId, string tokenHash, DateTime expiresAtUtc) : base(id)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id cannot be empty.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(tokenHash))
        {
            throw new ArgumentException("Token hash is required.", nameof(tokenHash));
        }

        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAtUtc = expiresAtUtc;
    }

    private RefreshToken() : base(Guid.NewGuid())
    {
        TokenHash = string.Empty;
    }

    public Guid UserId { get; private set; }

    public string TokenHash { get; private set; }

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime? RevokedAtUtc { get; private set; }

    public bool IsRevoked => RevokedAtUtc.HasValue;

    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;

    public static RefreshToken Create(Guid id, Guid userId, string tokenHash, DateTime expiresAtUtc)
    {
        if (expiresAtUtc == default)
        {
            throw new ArgumentException("Expiry date must be valid.", nameof(expiresAtUtc));
        }

        return new RefreshToken(id, userId, tokenHash, expiresAtUtc);
    }

    public void Revoke(DateTime revokedAtUtc)
    {
        if (RevokedAtUtc.HasValue)
        {
            return;
        }

        RevokedAtUtc = revokedAtUtc;
    }
}
