using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Domain.Transactions;
using PrevFinance.Infrastructure.Persistence;

namespace PrevFinance.Api.Finance;

public static class InstallmentEndpoints
{
    public static RouteGroupBuilder MapInstallmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/installments")
            .RequireAuthorization();

        group.MapPost("/", CreateAsync);

        return group;
    }

    [Authorize]
    private static async Task<IResult> CreateAsync(
        [FromBody] CreateInstallmentPlanRequest request,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        if (!TryReadUserId(claimsPrincipal, out var userId))
        {
            return Results.Unauthorized();
        }

        if (request.TotalAmount <= 0 || request.InstallmentCount <= 0 || string.IsNullOrWhiteSpace(request.Description))
        {
            return Results.BadRequest(new { error = "Invalid installment data." });
        }

        var account = await dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == request.AccountId, cancellationToken);
        if (account is null || account.UserId != userId)
        {
            return Results.NotFound();
        }

        var plan = InstallmentPlan.Create(
            Guid.NewGuid(),
            userId,
            request.AccountId,
            request.TotalAmount,
            request.InstallmentCount,
            request.StartDate,
            request.Frequency,
            request.Description,
            request.Type);

        var dueDates = ExpandDueDates(request.StartDate, request.InstallmentCount, request.Frequency);
        var amounts = SplitAmount(request.TotalAmount, request.InstallmentCount);

        var transactions = new List<Transaction>(request.InstallmentCount);
        for (var i = 0; i < request.InstallmentCount; i++)
        {
            var item = Transaction.Create(
                Guid.NewGuid(),
                userId,
                request.AccountId,
                amounts[i],
                dueDates[i],
                request.Type,
                request.Description,
                TransactionStatus.Pending,
                plan.Id,
                i + 1,
                request.InstallmentCount);

            transactions.Add(item);
        }

        dbContext.InstallmentPlans.Add(plan);
        dbContext.Transactions.AddRange(transactions);
        await dbContext.SaveChangesAsync(cancellationToken);

        var payload = new InstallmentPlanResponse(
            plan.Id,
            plan.AccountId,
            plan.TotalAmount,
            plan.InstallmentCount,
            plan.StartDate,
            plan.Frequency,
            plan.Description,
            plan.Type,
            transactions
                .OrderBy(x => x.InstallmentNumber)
                .Select(x => new InstallmentTransactionResponse(
                    x.Id,
                    x.InstallmentNumber ?? 0,
                    x.InstallmentCount ?? 0,
                    x.Amount,
                    x.OccurredOn,
                    x.Status))
                .ToList());

        return Results.Created($"/api/installments/{plan.Id}", payload);
    }

    private static List<DateOnly> ExpandDueDates(DateOnly startDate, int count, InstallmentFrequency frequency)
    {
        var dates = new List<DateOnly>(count);

        for (var i = 0; i < count; i++)
        {
            dates.Add(frequency switch
            {
                InstallmentFrequency.Weekly => startDate.AddDays(i * 7),
                _ => startDate.AddMonths(i)
            });
        }

        return dates;
    }

    private static List<decimal> SplitAmount(decimal totalAmount, int count)
    {
        var values = new List<decimal>(count);

        var baseValue = Math.Floor((totalAmount / count) * 100m) / 100m;
        for (var i = 0; i < count; i++)
        {
            values.Add(baseValue);
        }

        var allocated = baseValue * count;
        var differenceInCents = (int)Math.Round((totalAmount - allocated) * 100m, MidpointRounding.AwayFromZero);

        for (var i = 0; i < Math.Abs(differenceInCents); i++)
        {
            var index = i % count;
            values[index] += differenceInCents > 0 ? 0.01m : -0.01m;
        }

        return values;
    }

    private static bool TryReadUserId(ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        var claimValue = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(claimValue, out userId);
    }

    public sealed record CreateInstallmentPlanRequest(
        Guid AccountId,
        decimal TotalAmount,
        int InstallmentCount,
        DateOnly StartDate,
        InstallmentFrequency Frequency,
        string Description,
        TransactionType Type);

    public sealed record InstallmentPlanResponse(
        Guid Id,
        Guid AccountId,
        decimal TotalAmount,
        int InstallmentCount,
        DateOnly StartDate,
        InstallmentFrequency Frequency,
        string Description,
        TransactionType Type,
        IReadOnlyList<InstallmentTransactionResponse> Installments);

    public sealed record InstallmentTransactionResponse(
        Guid Id,
        int Number,
        int Total,
        decimal Amount,
        DateOnly DueOn,
        TransactionStatus Status);
}
