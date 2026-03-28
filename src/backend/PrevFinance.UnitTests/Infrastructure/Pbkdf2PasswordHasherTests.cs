using FluentAssertions;
using PrevFinance.Infrastructure.Security;

namespace PrevFinance.UnitTests.Infrastructure;

public class Pbkdf2PasswordHasherTests
{
    private readonly Pbkdf2PasswordHasher _sut = new();

    [Fact]
    public void HashAndVerify_ShouldReturnTrue_ForSamePassword()
    {
        var password = "StrongPass123";

        var hash = _sut.Hash(password);

        var isValid = _sut.Verify(password, hash);

        isValid.Should().BeTrue();
    }

    [Fact]
    public void Verify_ShouldReturnFalse_ForDifferentPassword()
    {
        var hash = _sut.Hash("StrongPass123");

        var isValid = _sut.Verify("WrongPass123", hash);

        isValid.Should().BeFalse();
    }
}
