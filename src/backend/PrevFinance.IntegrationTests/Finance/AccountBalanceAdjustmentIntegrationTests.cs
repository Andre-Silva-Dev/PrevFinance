using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PrevFinance.Api.Auth;
using PrevFinance.Api.Finance;
using PrevFinance.Domain.Accounts;
using PrevFinance.IntegrationTests.Infrastructure;

namespace PrevFinance.IntegrationTests.Finance;

public class AccountBalanceAdjustmentIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public AccountBalanceAdjustmentIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task RecalibrateBalance_ShouldUpdateCurrentBalance_AndKeepHistory()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var auth = await RegisterAsync(client, "balance-user@prevfinance.app", "Balance User");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var createResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Principal", AccountType.Checking, 1000m));
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await createResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();
        created.Should().NotBeNull();

        var firstAdjustResponse = await client.PostAsJsonAsync(
            $"/api/accounts/{created!.Id}/recalibrate",
            new AccountEndpoints.RecalibrateBalanceRequest(850m, "Ajuste pelo extrato"));

        firstAdjustResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondAdjustResponse = await client.PostAsJsonAsync(
            $"/api/accounts/{created.Id}/recalibrate",
            new AccountEndpoints.RecalibrateBalanceRequest(900m, "Correcao final"));

        secondAdjustResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var readResponse = await client.GetAsync($"/api/accounts/{created.Id}");
        readResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var account = await readResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();
        account.Should().NotBeNull();
        account!.InitialBalance.Should().Be(1000m);
        account.CurrentBalance.Should().Be(900m);

        var historyResponse = await client.GetAsync($"/api/accounts/{created.Id}/adjustments");
        historyResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var adjustments = await historyResponse.Content.ReadFromJsonAsync<List<AccountEndpoints.AccountBalanceAdjustmentResponse>>();
        adjustments.Should().NotBeNull();
        adjustments!.Should().HaveCount(2);

        adjustments[0].PreviousBalance.Should().Be(850m);
        adjustments[0].NewBalance.Should().Be(900m);
        adjustments[1].PreviousBalance.Should().Be(1000m);
        adjustments[1].NewBalance.Should().Be(850m);
    }

    [Fact]
    public async Task RecalibrateBalance_ShouldReturnNotFound_WhenAccountBelongsToAnotherUser()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var owner = await RegisterAsync(client, "owner-adjust@prevfinance.app", "Owner");
        var outsider = await RegisterAsync(client, "outsider-adjust@prevfinance.app", "Outsider");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", owner.AccessToken);
        var createResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Privada", AccountType.Savings, 500m));
        var created = await createResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", outsider.AccessToken);

        var adjustResponse = await client.PostAsJsonAsync(
            $"/api/accounts/{created!.Id}/recalibrate",
            new AccountEndpoints.RecalibrateBalanceRequest(100m, "Tentativa invalida"));

        var historyResponse = await client.GetAsync($"/api/accounts/{created.Id}/adjustments");

        adjustResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        historyResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
