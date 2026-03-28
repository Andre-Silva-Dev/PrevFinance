using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PrevFinance.Api.Auth;
using PrevFinance.Api.Finance;
using PrevFinance.Domain.Accounts;
using PrevFinance.IntegrationTests.Infrastructure;

namespace PrevFinance.IntegrationTests.Finance;

public class AccountIsolationIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public AccountIsolationIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Accounts_ShouldBeIsolatedPerAuthenticatedUser_WhenListingData()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var userA = await RegisterAsync(client, "tenant-a@prevfinance.app", "Tenant A");
        var userB = await RegisterAsync(client, "tenant-b@prevfinance.app", "Tenant B");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userA.AccessToken);
        var createResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta A", AccountType.Checking, 1000m));
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userB.AccessToken);
        var listResponse = await client.GetAsync("/api/accounts/");
        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var items = await listResponse.Content.ReadFromJsonAsync<List<AccountEndpoints.AccountResponse>>();
        items.Should().NotBeNull();
        items!.Should().BeEmpty();
    }

    [Fact]
    public async Task CrossUserResourceAccess_ShouldReturnNotFound_WhenAnotherUserTriesReadUpdateDelete()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var userA = await RegisterAsync(client, "tenant-owner@prevfinance.app", "Tenant Owner");
        var userB = await RegisterAsync(client, "tenant-outsider@prevfinance.app", "Tenant Outsider");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userA.AccessToken);
        var createResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Owner", AccountType.CreditCard, 500m));
        var createdAccount = await createResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userB.AccessToken);

        var readResponse = await client.GetAsync($"/api/accounts/{createdAccount!.Id}");
        var updateResponse = await client.PutAsJsonAsync($"/api/accounts/{createdAccount.Id}", new AccountEndpoints.UpdateAccountRequest("Conta Invasor"));
        var deleteResponse = await client.DeleteAsync($"/api/accounts/{createdAccount.Id}");

        readResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        deleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private static async Task<AuthEndpoints.AuthResponse> RegisterAsync(HttpClient client, string email, string name)
    {
        var response = await client.PostAsJsonAsync("/api/auth/register", new AuthEndpoints.RegisterRequest(email, name, "StrongPass123"));
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var payload = await response.Content.ReadFromJsonAsync<AuthEndpoints.AuthResponse>();
        payload.Should().NotBeNull();
        return payload!;
    }
}
