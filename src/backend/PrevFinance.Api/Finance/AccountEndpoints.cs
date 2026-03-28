using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Application.Abstractions;
using PrevFinance.Domain.Accounts;
using PrevFinance.Infrastructure.Persistence;

namespace PrevFinance.Api.Finance;

public static class AccountEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/accounts")
            .RequireAuthorization();

        group.MapPost("/", CreateAsync);
        group.MapGet("/", ListAsync);
        group.MapGet("/{accountId:guid}", GetByIdAsync);
        group.MapPut("/{accountId:guid}", UpdateAsync);
        group.MapPost("/{accountId:guid}/recalibrate", RecalibrateAsync);
        group.MapGet("/{accountId:guid}/adjustments", ListAdjustmentsAsync);
        group.MapDelete("/{accountId:guid}", DeleteAsync);

        return group;
    }

    [Authorize]
    private static async Task<IResult> CreateAsync(
        [FromBody] CreateAccountRequest request,
        ClaimsPrincipal claimsPrincipal,
        PrevFinanceDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (!TryReadUserId(claimsPrincipal, out var userId))
        {
            return Results.Unauthorized();
        }

        var account = Account.Create(Guid.NewGuid(), userId, request.Name, request.Type, request.InitialBalance);
        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Created($"/api/accounts/{account.Id}", ToResponse(account));
    }

    [Authorize]
    private static async Task<IResult> ListAsync(PrevFinanceDbContext dbContext, CancellationToken cancellationToken)
    {
        var items = await dbContext.Accounts
            .OrderBy(x => x.Name)
            .Select(x => ToResponse(x))
            .ToListAsync(cancellationToken);

        return Results.Ok(items);
    }

    [Authorize]
    private static async Task<IResult> GetByIdAsync(
        Guid accountId,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        if (account is null)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Access denied or account not found for account {AccountId}.", accountId);
            return Results.NotFound();
        }

        if (!TryReadUserId(claimsPrincipal, out var userId) || account.UserId != userId)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Cross-user access blocked for user {UserId} on account {AccountId}.", userId, accountId);
            return Results.NotFound();
        }

        return Results.Ok(ToResponse(account));
    }

    [Authorize]
    private static async Task<IResult> UpdateAsync(
        Guid accountId,
        [FromBody] UpdateAccountRequest request,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        if (account is null)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Update denied or account not found for account {AccountId}.", accountId);
            return Results.NotFound();
        }

        if (!TryReadUserId(claimsPrincipal, out var userId) || account.UserId != userId)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Cross-user update blocked for user {UserId} on account {AccountId}.", userId, accountId);
            return Results.NotFound();
        }

        account.Rename(request.Name);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(ToResponse(account));
    }

    [Authorize]
    private static async Task<IResult> RecalibrateAsync(
        Guid accountId,
        [FromBody] RecalibrateBalanceRequest request,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        ISystemClock clock,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        if (account is null)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Recalibration denied or account not found for account {AccountId}.", accountId);
            return Results.NotFound();
        }

        if (!TryReadUserId(claimsPrincipal, out var userId) || account.UserId != userId)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Cross-user recalibration blocked for user {UserId} on account {AccountId}.", userId, accountId);
            return Results.NotFound();
        }

        if (string.IsNullOrWhiteSpace(request.Reason))
        {
            return Results.BadRequest(new { error = "Adjustment reason is required." });
        }

        var previousBalance = account.RecalibrateBalance(request.NewBalance);
        var adjustment = AccountBalanceAdjustment.Create(
            Guid.NewGuid(),
            userId,
            account.Id,
            previousBalance,
            request.NewBalance,
            request.Reason,
            clock.UtcNow);

        dbContext.AccountBalanceAdjustments.Add(adjustment);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(ToResponse(account));
    }

    [Authorize]
    private static async Task<IResult> ListAdjustmentsAsync(
        Guid accountId,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        if (account is null)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("List adjustments denied or account not found for account {AccountId}.", accountId);
            return Results.NotFound();
        }

        if (!TryReadUserId(claimsPrincipal, out var userId) || account.UserId != userId)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Cross-user adjustment history blocked for user {UserId} on account {AccountId}.", userId, accountId);
            return Results.NotFound();
        }

        var items = await dbContext.AccountBalanceAdjustments
            .Where(x => x.AccountId == accountId)
            .OrderByDescending(x => x.AdjustedAtUtc)
            .Select(x => new AccountBalanceAdjustmentResponse(
                x.Id,
                x.AccountId,
                x.PreviousBalance,
                x.NewBalance,
                x.Reason,
                x.AdjustedAtUtc))
            .ToListAsync(cancellationToken);

        return Results.Ok(items);
    }

    [Authorize]
    private static async Task<IResult> DeleteAsync(
        Guid accountId,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken);
        if (account is null)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Delete denied or account not found for account {AccountId}.", accountId);
            return Results.NotFound();
        }

        if (!TryReadUserId(claimsPrincipal, out var userId) || account.UserId != userId)
        {
            loggerFactory.CreateLogger("SecurityAudit")
                .LogWarning("Cross-user delete blocked for user {UserId} on account {AccountId}.", userId, accountId);
            return Results.NotFound();
        }

        dbContext.Accounts.Remove(account);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }

    private static bool TryReadUserId(ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        var claimValue = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(claimValue, out userId);
    }

    private static AccountResponse ToResponse(Account account)
    {
        return new AccountResponse(account.Id, account.Name, account.Type, account.InitialBalance, account.CurrentBalance);
    }

    public sealed record CreateAccountRequest(string Name, AccountType Type, decimal InitialBalance);

    public sealed record UpdateAccountRequest(string Name);

    public sealed record RecalibrateBalanceRequest(decimal NewBalance, string Reason);

    public sealed record AccountResponse(Guid Id, string Name, AccountType Type, decimal InitialBalance, decimal CurrentBalance);

    public sealed record AccountBalanceAdjustmentResponse(
        Guid Id,
        Guid AccountId,
        decimal PreviousBalance,
        decimal NewBalance,
        string Reason,
        DateTime AdjustedAtUtc);
}
