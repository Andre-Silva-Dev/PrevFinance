namespace PrevFinance.Application.Abstractions;

public interface IJwtTokenService
{
    string GenerateAccessToken(Guid userId, string email);
}
