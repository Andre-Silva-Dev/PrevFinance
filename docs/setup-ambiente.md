# Checklist de Setup do Ambiente

## Pre-requisitos
- .NET SDK 10
- Node.js 22+
- npm 11+
- Docker Desktop

## Passo a passo (objetivo: < 30 minutos)
1. Copiar variaveis de ambiente:
   - Copy-Item .env.example .env
2. Subir dependencias locais:
   - ./scripts/dev-up.ps1
3. Restaurar e validar backend:
   - cd src/backend
   - dotnet tool restore
   - dotnet restore
   - dotnet test PrevFinance.slnx
4. Restaurar e validar frontend:
   - cd src/frontend/prevfinance-web
   - npm ci
   - npm run lint
   - npm run test -- --watch=false
   - npm run build

## Execucao local
- Backend:
  - ./scripts/backend-run.ps1
- Frontend:
  - ./scripts/frontend-run.ps1

## Encerramento
- ./scripts/dev-down.ps1

## Troubleshooting rapido
- Docker indisponivel: iniciar Docker Desktop e repetir scripts.
- Porta 5432 ocupada: alterar mapeamento no docker-compose.yml.
- Falha de migration: validar ConnectionStrings:DefaultConnection no appsettings.
