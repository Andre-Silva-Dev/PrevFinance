# Contexto de Desenvolvimento - PrevFinance

## Objetivo
Este arquivo registra o historico real de implementacao do projeto.
Use este documento para manter rastreabilidade entre planejamento, execucao e entrega.

## Regras de Atualizacao
- Atualizar ao final de cada task concluida.
- Registrar bloqueios tecnicos com causa e acao de contorno.
- Registrar decisoes arquiteturais que mudem o plano original.
- Manter linguagem curta e objetiva.

## Status Geral do Roadmap

| Fase | Nome | Status | Inicio | Fim | Responsavel | Observacoes |
| --- | --- | --- | --- | --- | --- | --- |
| 01 | Fundacao e Arquitetura | Concluida | 2026-03-27 | 2026-03-27 | Andre Silva | TASK-01, TASK-02 e TASK-03 concluidas com testes |
| 02 | Identidade e Multi-Tenancy | Concluida | 2026-03-28 | 2026-03-28 | Andre Silva | TASK-01, TASK-02 e TASK-03 concluidas com testes |
| 03 | Contas, Transacoes e Parcelamento | Nao iniciada | - | - | - | - |
| 04 | Projecoes e Dashboard | Nao iniciada | - | - | - | - |
| 05 | Quitacao de Dividas e Simulacoes | Nao iniciada | - | - | - | - |
| 06 | Hardening e Release MVP | Nao iniciada | - | - | - | - |

## Historico de Execucao

### Modelo de Registro
Data: AAAA-MM-DD
Fase: 00 - Nome da Fase
Task: TASK-00 - Nome da Task
Resumo: O que foi entregue em termos tecnicos
Evidencias: Link de PR, commit, screenshot, script ou log
Testes Unitarios: Resultado e cobertura dos cenarios de sucesso e erro
Testes de Integracao: Resultado e cobertura dos cenarios de sucesso e erro
Riscos: Risco residual apos entrega
Proximo passo: Qual task sera iniciada em seguida

---

### Entradas

#### 2026-03-27
Data: 2026-03-27
Fase: Planejamento
Task: Estruturacao inicial do plano
Resumo: Criacao do plano de desenvolvimento por fases independentes e tarefas separadas.
Evidencias: Estrutura de pastas em Fases e documentos de task.
Riscos: Nenhum risco tecnico identificado nesta etapa de planejamento.
Proximo passo: Iniciar Fase 01, TASK-01.

#### 2026-03-27
Data: 2026-03-27
Fase: 01 - Fundacao e Arquitetura
Task: TASK-01 - Definir Arquitetura e Solucao
Resumo: Criacao da solucao backend em camadas (Domain, Application, Infrastructure e Api), composicao de DI por extensoes, fronteiras de modulo (Auth, Finance, Projection e Debt) e estrutura frontend Angular por features.
Evidencias: src/backend/PrevFinance.slnx; src/backend/PrevFinance.Api/Program.cs; src/frontend/prevfinance-web/src/app/app.routes.ts; docs/arquitetura-fase-01.md.
Testes Unitarios: dotnet test src/backend/PrevFinance.slnx com 5 testes unitarios (sucesso e erro de validacao/DI) aprovados.
Testes de Integracao: dotnet test src/backend/PrevFinance.slnx com 2 testes de integracao de persistencia (sucesso e erro) aprovados.
Riscos: Integracao por Testcontainers depende de Docker ativo no host.
Proximo passo: Executar TASK-02 para ambiente e CI.

#### 2026-03-27
Data: 2026-03-27
Fase: 01 - Fundacao e Arquitetura
Task: TASK-02 - Preparar Ambiente e CI
Resumo: Ambiente local padronizado com docker-compose (PostgreSQL e Redis), scripts PowerShell para subir/parar/testar, lint frontend com angular-eslint e pipeline CI para push/PR com build, lint e testes de backend/frontend.
Evidencias: docker-compose.yml; scripts/dev-up.ps1; scripts/frontend-test.ps1; .github/workflows/ci.yml; docs/setup-ambiente.md.
Testes Unitarios: npm run test em src/frontend/prevfinance-web com 9 testes aprovados.
Testes de Integracao: dotnet test src/backend/PrevFinance.slnx com testes de integracao de persistencia aprovados.
Riscos: Sem Docker em execucao local, testes de integracao backend entram em fallback e nao exercitam banco real.
Proximo passo: Executar TASK-03 para modelagem e migration inicial.

#### 2026-03-27
Data: 2026-03-27
Fase: 01 - Fundacao e Arquitetura
Task: TASK-03 - Modelagem Inicial de Dados
Resumo: Implementacao das entidades User, Profile, Account e Transaction com auditoria (CreatedAtUtc/UpdatedAtUtc), mapeamentos EF Core com indices por usuario e periodo, migration inicial versionada e seed tecnico.
Evidencias: src/backend/PrevFinance.Infrastructure/Persistence/PrevFinanceDbContext.cs; src/backend/PrevFinance.Infrastructure/Persistence/Migrations/20260327173955_InitialCreate.cs; src/backend/scripts/seed-tech.sql.
Testes Unitarios: dotnet test src/backend/PrevFinance.slnx com validacoes de dominio e DI aprovadas.
Testes de Integracao: dotnet test src/backend/PrevFinance.slnx com cenarios de sucesso (persistencia e consulta) e erro (violacao de FK) aprovados.
Riscos: Nenhum risco tecnico residual relevante para a modelagem inicial.
Proximo passo: Iniciar Fase 02, TASK-01.

#### 2026-03-28
Data: 2026-03-28
Fase: 02 - Identidade e Multi-Tenancy
Task: TASK-01 - Implementar Cadastro e Login
Resumo: Implementacao de endpoints de autenticacao para cadastro, login, refresh e logout com JWT + refresh token, hash de senha PBKDF2, rota protegida /api/auth/me e migration para credenciais e refresh tokens.
Evidencias: src/backend/PrevFinance.Api/Auth/AuthEndpoints.cs; src/backend/PrevFinance.Api/Auth/JwtTokenService.cs; src/backend/PrevFinance.Infrastructure/Persistence/Migrations/20260328114530_AddAuthCredentialsAndRefreshTokens.cs; commit fe9a15f.
Testes Unitarios: dotnet test src/backend/PrevFinance.slnx com testes de hash de senha e validacoes de dominio (sucesso e erro) aprovados.
Testes de Integracao: dotnet test src/backend/PrevFinance.slnx com fluxo de cadastro/login/refresh/logout e erro de autenticacao aprovados.
Riscos: Chave JWT de desenvolvimento esta em appsettings e deve ser substituida por secret manager em ambiente produtivo.
Proximo passo: Executar TASK-02 para OAuth2 e perfis.

#### 2026-03-28
Data: 2026-03-28
Fase: 02 - Identidade e Multi-Tenancy
Task: TASK-02 - Integrar OAuth2 e Perfis
Resumo: Implementacao de callback OAuth2 por authorization code com provedor demo, vinculacao de conta externa ao usuario interno sem duplicar usuario por email, API de perfil (consulta e atualizacao) e migration para metadados externos.
Evidencias: src/backend/PrevFinance.Api/Auth/DemoOAuth2ProviderClient.cs; src/backend/PrevFinance.Api/Profile/ProfileEndpoints.cs; src/backend/PrevFinance.Infrastructure/Persistence/Migrations/20260328114745_AddOAuth2IdentitySupport.cs; commit a1e721b.
Testes Unitarios: dotnet test src/backend/PrevFinance.slnx com validacoes de dominio para update de perfil e suporte de credencial local/OAuth2 aprovados.
Testes de Integracao: dotnet test src/backend/PrevFinance.slnx com fluxo OAuth2 valido/invalido, ausencia de duplicacao por email e API de perfil (sucesso e erro) aprovados.
Riscos: Provedor OAuth2 demo e util para desenvolvimento e testes; integracao produtiva deve configurar provider externo real mantendo o contrato do callback.
Proximo passo: Executar TASK-03 para isolamento global por UserId.

#### 2026-03-28
Data: 2026-03-28
Fase: 02 - Identidade e Multi-Tenancy
Task: TASK-03 - Aplicar Isolamento Global por UserId
Resumo: Propagacao do contexto autenticado ate a camada de dados com filtro global por UserId em Profile, Account, Transaction e RefreshToken; endpoints de conta protegidos com guardas de ownership em read/update/delete e logs de auditoria para negacoes.
Evidencias: src/backend/PrevFinance.Infrastructure/Persistence/PrevFinanceDbContext.cs; src/backend/PrevFinance.Api/Finance/AccountEndpoints.cs; src/backend/PrevFinance.IntegrationTests/Finance/AccountIsolationIntegrationTests.cs; commit a8fd3fe.
Testes Unitarios: dotnet test src/backend/PrevFinance.slnx com cenarios de sucesso e erro de validacao de entidades (incluindo rename de conta) aprovados.
Testes de Integracao: dotnet test src/backend/PrevFinance.slnx com cenarios de acesso cruzado bloqueado (list/read/update/delete) e fluxo autorizado por usuario aprovados.
Riscos: Endpoints legacy que venham a ser adicionados sem autorizacao explicita podem contornar isolamento; manter politica de endpoints autenticados e testes de seguranca por recurso.
Proximo passo: Iniciar Fase 03, TASK-01.

#### 2026-03-28
Data: 2026-03-28
Fase: 02 - Identidade e Multi-Tenancy
Task: Complemento Frontend - Telas de Identidade, Perfil e Contas
Resumo: Implementacao das telas funcionais de autenticacao (login, cadastro e OAuth2 demo), perfil (consulta e atualizacao) e gestao de contas (listar, criar, renomear e excluir), com sessao persistida, interceptor de token com refresh e aderencia ao guia visual de identidade frontend.
Evidencias: src/frontend/src/app/features/auth/auth-shell/auth-shell.ts; src/frontend/src/app/features/finance/finance-shell/finance-shell.ts; src/frontend/src/app/core/auth/auth-api.service.ts; src/frontend/src/app/core/finance/accounts-api.service.ts.
Testes Unitarios: npm run test em src/frontend com 13 testes aprovados cobrindo componentes atualizados (auth, finance e navegacao).
Testes de Integracao: Validacao de fluxo frontend-endpoint via consumo real dos contratos HTTP de auth/profile/accounts (sucesso e erro) exercitados em tela e protegidos por interceptor de sessao.
Riscos: Para ambiente local, pode ser necessario configurar proxy de API no Angular para evitar problema de CORS quando backend e frontend estiverem em hosts/portas diferentes.
Proximo passo: Iniciar Fase 03, TASK-01 mantendo padrao de UI e TDD no frontend.

## Decisoes Arquiteturais Relevantes

| Data | Decisao | Impacto | Justificativa |
| --- | --- | --- | --- |
| 2026-03-27 | Adotar fases independentes por dominio funcional | Facilita paralelismo e previsibilidade de entrega | Alinhado ao PRD e ao MVP |
| 2026-03-28 | Isolamento multi-tenant aplicado por query filter global no DbContext com UserId do contexto autenticado | Reduz risco de vazamento entre tenants nas consultas da camada de dados | Alinha FR1/NFR2 com controle centralizado e testavel |

## Backlog de Riscos

| ID | Risco | Probabilidade | Impacto | Mitigacao | Status |
| --- | --- | --- | --- | --- | --- |
| R-01 | Divergencia entre regra de negocio e implementacao de projecao | Media | Alto | Testes de contrato + validacao com cenarios reais | Aberto |
| R-02 | Performance do dashboard acima de 2s sem cache valido | Media | Alto | Redis + precomputacao + invalidacao controlada | Aberto |
