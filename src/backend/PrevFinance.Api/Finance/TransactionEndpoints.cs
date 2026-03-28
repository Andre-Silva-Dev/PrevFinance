using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Domain.Transactions;
using PrevFinance.Infrastructure.Persistence;

namespace PrevFinance.Api.Finance;

public static class TransactionEndpoints
{
    public static RouteGroupBuilder MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transactions")
            .RequireAuthorization();

        group.MapPatch("/{transactionId:guid}", UpdateAsync);

        return group;
    }

    [Authorize]
    private static async Task<IResult> UpdateAsync(
        Guid transactionId,
        [FromBody] UpdateTransactionRequest request,
        PrevFinanceDbContext dbContext,
        ClaimsPrincipal claimsPrincipal,
        CancellationToken cancellationToken)
    {
        var transaction = await dbContext.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId, cancellationToken);
        if (transaction is null)
        {
            return Results.NotFound();
        }

        if (!TryReadUserId(claimsPrincipal, out var userId) || transaction.UserId != userId)
        {
            return Results.NotFound();
        }

        if (request.Amount.HasValue && request.Amount.Value <= 0)
        {
            return Results.BadRequest(new { error = "Amount must be greater than zero." });
        }

        if (request.ApplyToFutureInSeries && !transaction.InstallmentPlanId.HasValue)
        {
            return Results.BadRequest(new { error = "Cascade update is only available for installment transactions." });
        }

        var affected = new List<Transaction>();
        transaction.Update(request.Amount, request.DueOn, request.Description);
        affected.Add(transaction);

        if (request.ApplyToFutureInSeries && transaction.InstallmentPlanId.HasValue)
        {
            var plan = await dbContext.InstallmentPlans.FirstOrDefaultAsync(x => x.Id == transaction.InstallmentPlanId.Value, cancellationToken);
            if (plan is null)
            {
                return Results.NotFound();
            }

            var currentInstallmentNumber = transaction.InstallmentNumber ?? 0;
            var future = await dbContext.Transactions
                .Where(x => x.InstallmentPlanId == transaction.InstallmentPlanId && (x.InstallmentNumber ?? 0) > currentInstallmentNumber)
                .OrderBy(x => x.InstallmentNumber)
                .ToListAsync(cancellationToken);

            if (future.Any(x => x.Status is TransactionStatus.Paid or TransactionStatus.Cancelled))
            {
                return Results.Conflict(new { error = "Cannot apply cascade because part of the future series is already finalized." });
            }

            foreach (var item in future)
            {
                DateOnly? dueOn = null;
                if (request.DueOn.HasValue)
                {
                    var offset = (item.InstallmentNumber ?? currentInstallmentNumber) - currentInstallmentNumber;
                    dueOn = AddFrequency(request.DueOn.Value, offset, plan.Frequency);
                }

                item.Update(request.Amount, dueOn, request.Description);
                affected.Add(item);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new UpdatedTransactionBatchResponse(
            affected
                .OrderBy(x => x.InstallmentNumber ?? int.MaxValue)
                .Select(x => new UpdatedTransactionResponse(
                    x.Id,
                    x.AccountId,
                    x.Amount,
                    x.OccurredOn,
                    x.Description,
                    x.Status,
                    x.InstallmentPlanId,
                    x.InstallmentNumber,
                    x.InstallmentCount))
                .ToList()));
    }

    private static DateOnly AddFrequency(DateOnly baseDate, int offset, InstallmentFrequency frequency)
    {
        return frequency switch
        {
            InstallmentFrequency.Weekly => baseDate.AddDays(offset * 7),
            _ => baseDate.AddMonths(offset)
        };
    }

    private static bool TryReadUserId(ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        var claimValue = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(claimValue, out userId);
    }

    public sealed record UpdateTransactionRequest(decimal? Amount, DateOnly? DueOn, string? Description, bool ApplyToFutureInSeries);

    public sealed record UpdatedTransactionBatchResponse(IReadOnlyList<UpdatedTransactionResponse> Transactions);

    public sealed record UpdatedTransactionResponse(
        Guid Id,
        Guid AccountId,
        decimal Amount,
        DateOnly DueOn,
        string Description,
        TransactionStatus Status,
        Guid? InstallmentPlanId,
        int? InstallmentNumber,
        int? InstallmentCount);
}
