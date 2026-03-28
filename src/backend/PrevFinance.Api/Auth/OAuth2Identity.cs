namespace PrevFinance.Api.Auth;

public sealed record OAuth2Identity(string Subject, string Email, string FullName);
