namespace PrevFinance.Application.Abstractions;

public interface ISystemClock
{
    DateTime UtcNow { get; }
}
