using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrevFinance.Application.Abstractions;
using PrevFinance.Domain.Users;
using PrevFinance.Infrastructure.Persistence;
using PrevFinance.Infrastructure.Security;

namespace PrevFinance.Api.Auth;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", RegisterAsync);
        group.MapPost("/login", LoginAsync);
        group.MapPost("/refresh", RefreshAsync);
        group.MapPost("/logout", LogoutAsync)
            .RequireAuthorization();
        group.MapGet("/me", GetMeAsync)
            .RequireAuthorization();

        return group;
    }

    private static async Task<IResult> RegisterAsync(
        [FromBody] RegisterRequest request,
        PrevFinanceDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ISystemClock clock,
        JwtOptions jwtOptions,
        CancellationToken cancellationToken)
    {
        if (!IsValidPassword(request.Password))
        {
            return Results.BadRequest(new { error = "Password must have at least 8 characters." });
        }

        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var existingUser = await dbContext.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (existingUser)
        {
            return Results.Conflict(new { error = "A user with this e-mail already exists." });
        }

        var user = User.CreateWithPassword(Guid.NewGuid(), normalizedEmail, passwordHasher.Hash(request.Password));
        var profile = Profile.Create(Guid.NewGuid(), user.Id, request.FullName);

        dbContext.Users.Add(user);
        dbContext.Profiles.Add(profile);

        var tokenPair = await IssueTokenPairAsync(user, dbContext, jwtTokenService, clock, jwtOptions, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new AuthResponse(user.Id, user.Email, tokenPair.AccessToken, tokenPair.RefreshToken, tokenPair.RefreshTokenExpiresAtUtc));
    }

    private static async Task<IResult> LoginAsync(
        [FromBody] LoginRequest request,
        PrevFinanceDbContext dbContext,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        ISystemClock clock,
        JwtOptions jwtOptions,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);
        if (user is null || string.IsNullOrWhiteSpace(user.PasswordHash) || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        var tokenPair = await IssueTokenPairAsync(user, dbContext, jwtTokenService, clock, jwtOptions, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new AuthResponse(user.Id, user.Email, tokenPair.AccessToken, tokenPair.RefreshToken, tokenPair.RefreshTokenExpiresAtUtc));
    }

    private static async Task<IResult> RefreshAsync(
        [FromBody] RefreshRequest request,
        PrevFinanceDbContext dbContext,
        IJwtTokenService jwtTokenService,
        ISystemClock clock,
        JwtOptions jwtOptions,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Results.Unauthorized();
        }

        var tokenHash = Sha256TokenHasher.Hash(request.RefreshToken);
        var refreshToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash, cancellationToken);

        var now = clock.UtcNow;
        if (refreshToken is null || refreshToken.IsRevoked || refreshToken.IsExpired(now))
        {
            return Results.Unauthorized();
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == refreshToken.UserId, cancellationToken);
        if (user is null)
        {
            return Results.Unauthorized();
        }

        refreshToken.Revoke(now);

        var tokenPair = await IssueTokenPairAsync(user, dbContext, jwtTokenService, clock, jwtOptions, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.Ok(new AuthResponse(user.Id, user.Email, tokenPair.AccessToken, tokenPair.RefreshToken, tokenPair.RefreshTokenExpiresAtUtc));
    }

    private static async Task<IResult> LogoutAsync(
        [FromBody] RefreshRequest request,
        ClaimsPrincipal claimsPrincipal,
        PrevFinanceDbContext dbContext,
        ISystemClock clock,
        CancellationToken cancellationToken)
    {
        var userIdClaim = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdClaim, out var userId) || string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return Results.Unauthorized();
        }

        var tokenHash = Sha256TokenHasher.Hash(request.RefreshToken);
        var refreshToken = await dbContext.RefreshTokens
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash && x.UserId == userId, cancellationToken);

        if (refreshToken is null)
        {
            return Results.NotFound();
        }

        refreshToken.Revoke(clock.UtcNow);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }

    [Authorize]
    private static IResult GetMeAsync(ClaimsPrincipal claimsPrincipal)
    {
        var userId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        return Results.Ok(new { userId, email });
    }

    private static async Task<TokenPair> IssueTokenPairAsync(
        User user,
        PrevFinanceDbContext dbContext,
        IJwtTokenService jwtTokenService,
        ISystemClock clock,
        JwtOptions jwtOptions,
        CancellationToken cancellationToken)
    {
        var now = clock.UtcNow;
        var accessToken = jwtTokenService.GenerateAccessToken(user.Id, user.Email);

        var refreshTokenRaw = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenEntity = RefreshToken.Create(
            Guid.NewGuid(),
            user.Id,
            Sha256TokenHasher.Hash(refreshTokenRaw),
            now.AddDays(jwtOptions.RefreshTokenDays));

        await dbContext.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken);

        return new TokenPair(accessToken, refreshTokenRaw, refreshTokenEntity.ExpiresAtUtc);
    }

    private static bool IsValidPassword(string password)
    {
        return !string.IsNullOrWhiteSpace(password) && password.Trim().Length >= 8;
    }

    public sealed record RegisterRequest(string Email, string FullName, string Password);

    public sealed record LoginRequest(string Email, string Password);

    public sealed record RefreshRequest(string RefreshToken);

    public sealed record AuthResponse(Guid UserId, string Email, string AccessToken, string RefreshToken, DateTime RefreshTokenExpiresAtUtc);

    private sealed record TokenPair(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiresAtUtc);
}
