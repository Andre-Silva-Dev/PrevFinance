using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Users;

public sealed class User : AuditableEntity
{
    private User(Guid id, string email, string? passwordHash) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
    }

    private User() : base(Guid.NewGuid())
    {
        Email = string.Empty;
        PasswordHash = null;
    }

    public string Email { get; private set; }

    public string? PasswordHash { get; private set; }

    public static User Create(Guid id, string email)
    {
        ValidateEmail(email);
        return new User(id, email.Trim().ToLowerInvariant(), null);
    }

    public static User CreateWithPassword(Guid id, string email, string passwordHash)
    {
        ValidateEmail(email);

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        }

        return new User(id, email.Trim().ToLowerInvariant(), passwordHash);
    }

    public void UpdatePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        }

        PasswordHash = passwordHash;
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("A valid e-mail is required.", nameof(email));
        }
    }
}
