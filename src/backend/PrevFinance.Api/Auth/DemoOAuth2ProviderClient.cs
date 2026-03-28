using System.Text;
using System.Text.Json;

namespace PrevFinance.Api.Auth;

public sealed class DemoOAuth2ProviderClient : IOAuth2ProviderClient
{
    public string ProviderName => "demo";

    public Task<OAuth2Identity?> ExchangeCodeAsync(string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Task.FromResult<OAuth2Identity?>(null);
        }

        try
        {
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            var payload = JsonSerializer.Deserialize<DemoCodePayload>(json);

            if (payload is null || string.IsNullOrWhiteSpace(payload.Subject) || string.IsNullOrWhiteSpace(payload.Email) || string.IsNullOrWhiteSpace(payload.FullName))
            {
                return Task.FromResult<OAuth2Identity?>(null);
            }

            return Task.FromResult<OAuth2Identity?>(new OAuth2Identity(payload.Subject.Trim(), payload.Email.Trim().ToLowerInvariant(), payload.FullName.Trim()));
        }
        catch
        {
            return Task.FromResult<OAuth2Identity?>(null);
        }
    }

    private sealed record DemoCodePayload(string Subject, string Email, string FullName);
}
