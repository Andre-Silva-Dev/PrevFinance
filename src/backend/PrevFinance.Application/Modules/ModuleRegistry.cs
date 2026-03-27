using PrevFinance.Application.Modules.Auth;
using PrevFinance.Application.Modules.Debt;
using PrevFinance.Application.Modules.Finance;
using PrevFinance.Application.Modules.Projection;

namespace PrevFinance.Application.Modules;

public static class ModuleRegistry
{
    public static IReadOnlyCollection<IPrevFinanceModule> All { get; } =
    [
        new AuthModule(),
        new FinanceModule(),
        new ProjectionModule(),
        new DebtModule()
    ];
}
