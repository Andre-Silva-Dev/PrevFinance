using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Api.Auth;
using PrevFinance.Api.Finance;
using PrevFinance.Domain.Accounts;
using PrevFinance.Domain.Transactions;
using PrevFinance.IntegrationTests.Infrastructure;

namespace PrevFinance.IntegrationTests.Finance;

public class TransactionCascadeAndOverdueIntegrationTests : IClassFixture<PostgreSqlFixture>
{
    private readonly PostgreSqlFixture _fixture;

    public TransactionCascadeAndOverdueIntegrationTests(PostgreSqlFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task UpdateTransaction_ShouldApplyCascade_WhenRequestedForInstallmentSeries()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var auth = await RegisterAsync(client, "cascade-user@prevfinance.app", "Cascade User");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var createAccountResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Cascade", AccountType.CreditCard, 2000m));
        var account = await createAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        var createPlanResponse = await client.PostAsJsonAsync(
            "/api/installments/",
            new InstallmentEndpoints.CreateInstallmentPlanRequest(
                account!.Id,
                400m,
                4,
                new DateOnly(2026, 1, 10),
                InstallmentFrequency.Monthly,
                "Compra",
                TransactionType.Expense));

        var plan = await createPlanResponse.Content.ReadFromJsonAsync<InstallmentEndpoints.InstallmentPlanResponse>();
        var secondInstallment = plan!.Installments.Single(x => x.Number == 2);

        var updateResponse = await client.PatchAsJsonAsync(
            $"/api/transactions/{secondInstallment.Id}",
            new TransactionEndpoints.UpdateTransactionRequest(150m, new DateOnly(2026, 2, 20), "Compra ajustada", true));

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await updateResponse.Content.ReadFromJsonAsync<TransactionEndpoints.UpdatedTransactionBatchResponse>();
        updated.Should().NotBeNull();
        updated!.Transactions.Should().HaveCount(3);

        updated.Transactions.Single(x => x.InstallmentNumber == 2).Amount.Should().Be(150m);
        updated.Transactions.Single(x => x.InstallmentNumber == 2).DueOn.Should().Be(new DateOnly(2026, 2, 20));
        updated.Transactions.Single(x => x.InstallmentNumber == 3).DueOn.Should().Be(new DateOnly(2026, 3, 20));
        updated.Transactions.Single(x => x.InstallmentNumber == 4).DueOn.Should().Be(new DateOnly(2026, 4, 20));
    }

    [Fact]
    public async Task UpdateTransaction_ShouldReturnConflict_WhenFutureInstallmentIsFinalized()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var auth = await RegisterAsync(client, "cascade-conflict@prevfinance.app", "Cascade Conflict");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var createAccountResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Cascade", AccountType.CreditCard, 2000m));
        var account = await createAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        var createPlanResponse = await client.PostAsJsonAsync(
            "/api/installments/",
            new InstallmentEndpoints.CreateInstallmentPlanRequest(
                account!.Id,
                300m,
                3,
                new DateOnly(2026, 1, 15),
                InstallmentFrequency.Monthly,
                "Curso",
                TransactionType.Expense));

        var plan = await createPlanResponse.Content.ReadFromJsonAsync<InstallmentEndpoints.InstallmentPlanResponse>();

        await using (var db = _fixture.CreateDbContext())
        {
            var third = await db.Transactions.FirstAsync(x => x.Id == plan!.Installments.Single(i => i.Number == 3).Id);
            db.Entry(third).Property(x => x.Status).CurrentValue = TransactionStatus.Paid;
            await db.SaveChangesAsync();
        }

        var firstInstallment = plan!.Installments.Single(x => x.Number == 1);
        var updateResponse = await client.PatchAsJsonAsync(
            $"/api/transactions/{firstInstallment.Id}",
            new TransactionEndpoints.UpdateTransactionRequest(110m, null, null, true));

        updateResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task GetAccount_ShouldMarkOverdueAndReflectEffectiveBalance_WhenPendingInstallmentIsPastDue()
    {
        if (!_fixture.IsAvailable)
        {
            return;
        }

        await using var factory = new ApiWebApplicationFactory(_fixture.ConnectionString);
        using var client = factory.CreateClient();

        var auth = await RegisterAsync(client, "overdue-user@prevfinance.app", "Overdue User");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

        var createAccountResponse = await client.PostAsJsonAsync("/api/accounts/", new AccountEndpoints.CreateAccountRequest("Conta Overdue", AccountType.Checking, 1000m));
        var account = await createAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();

        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-1));
        var createPlanResponse = await client.PostAsJsonAsync(
            "/api/installments/",
            new InstallmentEndpoints.CreateInstallmentPlanRequest(
                account!.Id,
                200m,
                1,
                yesterday,
                InstallmentFrequency.Monthly,
                "Despesa atrasada",
                TransactionType.Expense));

        var plan = await createPlanResponse.Content.ReadFromJsonAsync<InstallmentEndpoints.InstallmentPlanResponse>();
        plan.Should().NotBeNull();

        var readAccountResponse = await client.GetAsync($"/api/accounts/{account.Id}");
        readAccountResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedAccount = await readAccountResponse.Content.ReadFromJsonAsync<AccountEndpoints.AccountResponse>();
        updatedAccount.Should().NotBeNull();
        updatedAccount!.EffectiveBalance.Should().Be(800m);

        await using var db = _fixture.CreateDbContext();
        var storedTransaction = await db.Transactions.FirstAsync(x => x.Id == plan!.Installments.Single().Id);
        storedTransaction.Status.Should().Be(TransactionStatus.Overdue);
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
