using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Infrastructure.Persistence;

namespace PrevFinance.Api.Profile;

public static class ProfileEndpoints
{
    public static RouteGroupBuilder MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/profile")
            .RequireAuthorization();

        group.MapGet("/me", GetMeAsync);
        group.MapPut("/me", UpdateMeAsync);

        return group;
    }

    [Authorize]
    private static async Task<IResult> GetMeAsync(
        ClaimsPrincipal claimsPrincipal,
        PrevFinanceDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var userIdClaim = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }

        var profile = await dbContext.Profiles.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (profile is null || user is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new ProfileResponse(user.Id, user.Email, profile.FullName));
    }

    [Authorize]
    private static async Task<IResult> UpdateMeAsync(
        [FromBody] UpdateProfileRequest request,
        ClaimsPrincipal claimsPrincipal,
        PrevFinanceDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return Results.BadRequest(new { error = "Full name is required." });
        }

        var userIdClaim = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Results.Unauthorized();
        }

        var profile = await dbContext.Profiles.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        if (profile is null)
        {
            return Results.NotFound();
        }

        profile.UpdateFullName(request.FullName);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new ProfileResponse(userId, claimsPrincipal.FindFirstValue(ClaimTypes.Email) ?? string.Empty, profile.FullName));
    }

    public sealed record UpdateProfileRequest(string FullName);

    public sealed record ProfileResponse(Guid UserId, string Email, string FullName);
}
