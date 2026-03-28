namespace PrevFinance.Application.Abstractions;

public interface ICurrentUserContext
{
    Guid? UserId { get; }
}
