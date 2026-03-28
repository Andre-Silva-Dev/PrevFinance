namespace PrevFinance.Api.Auth;

public interface IOAuth2ProviderClient
{
    string ProviderName { get; }

    Task<OAuth2Identity?> ExchangeCodeAsync(string code, CancellationToken cancellationToken);
}
