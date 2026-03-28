using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PrevFinance.Api.Auth;
using PrevFinance.IntegrationTests.Infrastructure;

namespace PrevFinance.IntegrationTests.Auth;

public class AuthApiIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public AuthApiIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Register_ShouldReturnAccessAndRefreshToken_WhenDataIsValid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest("new-user@prevfinance.app", "New User", "StrongPass123"));

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var payload = await response.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();
        payload.Should().NotBeNull();
        payload!.AccessToken.Should().NotBeNullOrWhiteSpace();
        payload.RefreshToken.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task LoginAndMe_ShouldReturnAuthorizedUser_WhenCredentialsAreValid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest("auth-me@prevfinance.app", "Auth Me", "StrongPass123"));

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new AuthEndpoints.LoginRequest("auth-me@prevfinance.app", "StrongPass123"));
        loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var loginPayload = await loginResponse.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();
        loginPayload.Should().NotBeNull();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginPayload!.AccessToken);
        var meResponse = await client.GetAsync("/api/auth/me");

        meResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsInvalid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest("auth-fail@prevfinance.app", "Auth Fail", "StrongPass123"));

        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new AuthEndpoints.LoginRequest("auth-fail@prevfinance.app", "InvalidPass123"));

        loginResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task RefreshAndLogout_ShouldRotateAndRevokeToken_WhenFlowIsValid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var registerResponse = await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest("refresh@prevfinance.app", "Refresh User", "StrongPass123"));
        var registerPayload = await registerResponse.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();
        registerPayload.Should().NotBeNull();

        var firstRefreshResponse = await client.PostAsJsonAsync("/api/auth/refresh", new AuthEndpoints.RefreshRequest(registerPayload!.RefreshToken));
        firstRefreshResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var refreshedPayload = await firstRefreshResponse.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();
        refreshedPayload.Should().NotBeNull();

        var replayRefreshResponse = await client.PostAsJsonAsync("/api/auth/refresh", new AuthEndpoints.RefreshRequest(registerPayload.RefreshToken));
        replayRefreshResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", refreshedPayload!.AccessToken);
        var logoutResponse = await client.PostAsJsonAsync("/api/auth/logout", new AuthEndpoints.RefreshRequest(refreshedPayload.RefreshToken));
        logoutResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var postLogoutRefresh = await client.PostAsJsonAsync("/api/auth/refresh", new AuthEndpoints.RefreshRequest(refreshedPayload.RefreshToken));
        postLogoutRefresh.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
