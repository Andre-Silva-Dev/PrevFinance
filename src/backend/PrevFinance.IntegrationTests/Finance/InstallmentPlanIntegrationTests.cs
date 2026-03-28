using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using PrevFinance.Api.Auth;
using PrevFinance.Api.Finance;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Transactions;
using PrevFinance.IntegrationTests.Infrastructure;

namespace PrevFinance.IntegrationTests.Finance;

public class InstallmentPlanIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public InstallmentPlanIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task CreateInstallmentPlan_ShouldGenerateExpectedTransactions_WhenRequestIsValid()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var auth = await RegisterAsync(client, "installment-user@prevfinance.app", "Installment User");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var createAccountResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Parcela", AccountType.CreditCard, 3000m));
        var account = await createAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        var request = new InstallmentEndpoints.CreateInstallmentPlanRequest(
            account!.Id,
            1200m,
            12,
            new DateOnly(2026, 1, 31),
            InstallmentFrequency.Monthly,
            "Notebook",
            TransactionType.Expense);

        var response = await client.PostAsJsonAsync("/api/installments/", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var payload = await response.Content.ReadFromJsonAsync<InstallmentEndpoints.InstallmentPlanResponse>();
        payload.Should().NotBeNull();
        payload!.Installments.Should().HaveCount(12);
        payload.Installments.Sum(x => x.Amount).Should().Be(1200m);
        payload.Installments[0].DueOn.Should().Be(new DateOnly(2026, 1, 31));
        payload.Installments[1].DueOn.Should().Be(new DateOnly(2026, 2, 28));
        payload.Installments[2].DueOn.Should().Be(new DateOnly(2026, 3, 31));
        payload.Installments.All(x => x.Status == TransactionStatus.Pending).Should().BeTrue();
    }

    [Fact]
    public async Task CreateInstallmentPlan_ShouldReturnBadRequest_WhenValidationFails()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var auth = await RegisterAsync(client, "installment-invalid@prevfinance.app", "Installment Invalid");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var createAccountResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta", AccountType.Checking, 1500m));
        var account = await createAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        var request = new InstallmentEndpoints.CreateInstallmentPlanRequest(
            account!.Id,
            0m,
            0,
            new DateOnly(2026, 4, 1),
            InstallmentFrequency.Monthly,
            "Curso",
            TransactionType.Expense);

        var response = await client.PostAsJsonAsync("/api/installments/", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateInstallmentPlan_ShouldReturnNotFound_WhenAccountBelongsToAnotherUser()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var owner = await RegisterAsync(client, "installment-owner@prevfinance.app", "Owner");
        var outsider = await RegisterAsync(client, "installment-outsider@prevfinance.app", "Outsider");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", owner.AccessToken);
        var createAccountResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Owner", AccountType.Wallet, 400m));
        var account = await createAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", outsider.AccessToken);
        var request = new InstallmentEndpoints.CreateInstallmentPlanRequest(
            account!.Id,
            300m,
            3,
            new DateOnly(2026, 5, 15),
            InstallmentFrequency.Monthly,
            "Compra privada",
            TransactionType.Expense);

        var response = await client.PostAsJsonAsync("/api/installments/", request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
