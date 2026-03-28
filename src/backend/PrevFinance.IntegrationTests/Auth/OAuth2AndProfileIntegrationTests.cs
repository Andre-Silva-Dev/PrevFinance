using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using PrevFinance.Api.Auth;
using PrevFinance.Api.Profile;
using PrevFinance.IntegrationTests.Infrastructure;

namespace PrevFinance.IntegrationTests.Auth;

public class OAuth2AndProfileIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public OAuth2AndProfileIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task OAuth2Callback_ShouldCreateAndAuthenticateUser_WhenCodeIsValid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var code = BuildDemoCode("oauth-sub-01", "oauth.user@prevfinance.app", "OAuth User");
        var response = await client.PostAsJsonAsync("/api/auth/oauth2/demo/callback", new AuthEndpoints.OAuth2CallbackRequest(code));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();
        payload.Should().NotBeNull();
        payload!.Email.Should().Be("oauth.user@prevfinance.app");
    }

    [Fact]
    public async Task OAuth2Callback_ShouldNotDuplicateUser_WhenEmailAlreadyExists()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest("merge.email@prevfinance.app", "Merge Email", "StrongPass123"));
        var registerPayload = await registerResponse.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();

        var code = BuildDemoCode("oauth-sub-02", "merge.email@prevfinance.app", "Merge Email OAuth");
        var oauthResponse = await client.PostAsJsonAsync("/api/auth/oauth2/demo/callback", new AuthEndpoints.OAuth2CallbackRequest(code));
        var oauthPayload = await oauthResponse.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();

        oauthResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        oauthPayload.Should().NotBeNull();
        oauthPayload!.UserId.Should().Be(registerPayload!.UserId);
    }

    [Fact]
    public async Task ProfileApi_ShouldGetAndUpdateProfile_WhenUserIsAuthenticated()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest("profile.user@prevfinance.app", "Profile User", "StrongPass123"));
        var payload = await registerResponse.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", payload!.AccessToken);

        var profileResponse = await client.GetAsync("/api/profile/me");
        profileResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updateResponse = await client.PutAsJsonAsync("/api/profile/me", new ProfileEndpoints.UpdateProfileRequest("Profile User Updated"));
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedPayload = await updateResponse.Content.ReadFromJsonAsync<ProfileEndpoints.ProfileResponse>();
        updatedPayload.Should().NotBeNull();
        updatedPayload!.FullName.Should().Be("Profile User Updated");
    }

    [Fact]
    public async Task OAuth2Callback_ShouldReturnUnauthorized_WhenCodeIsInvalid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/oauth2/demo/callback", new AuthEndpoints.OAuth2CallbackRequest("invalid"));
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static string BuildDemoCode(string subject, string email, string fullName)
    {
        var payload = JsonSerializer.Serialize(new { Subject = subject, Email = email, FullName = fullName });
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(payload));
    }
}
