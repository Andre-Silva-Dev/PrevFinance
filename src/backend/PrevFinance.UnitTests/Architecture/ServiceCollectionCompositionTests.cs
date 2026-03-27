using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrevFinance.Application.DependencyInjection;
using PrevFinance.Infrastructure.DependencyInjection;
using PrevFinance.Infrastructure.Persistence;

namespace PrevFinance.UnitTests.Architecture;

public class ServiceCollectionCompositionTests
{
    [Fact]
    public void AddPrevFinanceInfrastructure_ShouldThrow_WhenDefaultConnectionIsMissing()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var action = () => services
            .AddPrevFinanceApplication()
            .AddPrevFinanceInfrastructure(configuration);

        // Assert
        action
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Connection string 'DefaultConnection' was not configured.*");
    }

    [Fact]
    public void AddPrevFinanceInfrastructure_ShouldRegisterDbContext_WhenConnectionExists()
    {
        // Arrange
        var settings = new Dictionary<string, string?>
        {
            ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Port=5432;Database=prevfinance_test;Username=postgres;Password=postgres"
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var services = new ServiceCollection();

        // Act
        services
            .AddPrevFinanceApplication()
            .AddPrevFinanceInfrastructure(configuration);

        using var serviceProvider = services.BuildServiceProvider();

        // Assert
        var dbContext = serviceProvider.GetService<PrevFinanceDbContext>();
        dbContext.Should().NotBeNull();
    }
}
