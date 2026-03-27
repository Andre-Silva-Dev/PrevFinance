# Arquitetura da Fundacao (Fase 01)

## Visao Geral
A fundacao adota o padrao Modular Monolith com Clean Architecture.

Estrutura principal:
- Backend: src/backend
- Frontend: src/frontend/prevfinance-web

## Camadas do Backend
- PrevFinance.Domain: entidades, regras de validacao e tipos de dominio.
- PrevFinance.Application: composicao de casos de uso, contratos e modulos.
- PrevFinance.Infrastructure: persistencia, DbContext, mapeamentos e seed tecnico.
- PrevFinance.Api: composicao HTTP, health checks, migration e bootstrap.

Fluxo entre camadas:

API -> Application -> Domain
API -> Infrastructure -> Domain
Application -> Domain
Infrastructure -> Application e Domain

Regras de dependencia:
- Domain nao depende de nenhuma camada interna.
- Application depende apenas de Domain.
- Infrastructure depende de Application e Domain.
- Api depende de Application e Infrastructure.

## Fronteiras de Modulo
Modulos declarados na fase:
- Auth
- Finance
- Projection
- Debt

No backend, as fronteiras estao em Application/Modules.
No frontend, as features estao em src/app/features.

## Composicao de Servicos (DI)
- AddPrevFinanceApplication registra servicos da camada de aplicacao.
- AddPrevFinanceInfrastructure registra DbContext e seed tecnico.
- Program.cs aplica migration automaticamente no startup.

## Fluxo de Inicializacao
1. API carrega configuracao.
2. Registra servicos de Application e Infrastructure.
3. Executa migration pendente.
4. Opcionalmente executa seed tecnico em Development.
5. Expoe endpoints basicos de health.

## Persistencia Inicial
Entidades base mapeadas no PostgreSQL:
- users
- profiles
- accounts
- transactions

Convencoes aplicadas:
- Chave primaria Guid em todas as tabelas.
- Auditoria: created_at_utc, updated_at_utc.
- Multi-tenant: UserId em entidades de dominio multi-tenant.
- Indices iniciais por usuario e periodo para consultas frequentes.
