using PrevFinance.Domain.Common;

namespace PrevFinance.Domain.Users;

public sealed class User : AuditableEntity
{
    private User(Guid id, string email) : base(id)
    {
        Email = email;
    }

    private User() : base(Guid.NewGuid())
    {
        Email = string.Empty;
    }

    public string Email { get; private set; }

    public static User Create(Guid id, string email)
    {
        ValidateEmail(email);
        return new User(id, email.Trim().ToLowerInvariant());
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            throw new ArgumentException("A valid e-mail is required.", nameof(email));
        }
    }
}
