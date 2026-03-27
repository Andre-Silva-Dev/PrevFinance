using Microsoft.Extensions.DependencyInjection;
using PrevFinance.Application.Abstractions;
using PrevFinance.Application.Services;

namespace PrevFinance.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPrevFinanceApplication(this IServiceCollection services)
    {
        services.AddScoped<ISystemClock, SystemClock>();
        return services;
    }
}
