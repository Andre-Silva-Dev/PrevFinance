using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrevFinance.Application.Abstractions;
using PrevFinance.Infrastructure.Persistence;
using PrevFinance.Infrastructure.Persistence.Seeding;
using PrevFinance.Infrastructure.Security;

namespace PrevFinance.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPrevFinanceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'DefaultConnection' was not configured.");
        }

        services.AddDbContext<PrevFinanceDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.MigrationsAssembly(typeof(PrevFinanceDbContext).Assembly.FullName)));

        services.AddScoped<ISeedDataService, TechnicalSeedDataService>();
        services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();

        return services;
    }
}
