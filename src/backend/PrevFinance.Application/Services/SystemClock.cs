using PrevFinance.Application.Abstractions;

namespace PrevFinance.Application.Services;

public sealed class SystemClock : ISystemClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
