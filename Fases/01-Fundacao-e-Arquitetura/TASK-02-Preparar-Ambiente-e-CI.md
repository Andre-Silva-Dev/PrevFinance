# TASK-02 - Preparar Ambiente e CI

## Objetivo
Garantir ambiente de desenvolvimento padrao e pipeline de qualidade desde o inicio.

## Entregaveis
- Ambiente local padronizado para backend, frontend, PostgreSQL e Redis.
- Pipeline CI com build e testes basicos.
- Arquivos de configuracao de lint e formatacao.

## Passos Tecnicos
1. Criar docker-compose para Postgres e Redis.
2. Definir scripts de inicializacao no backend e frontend.
3. Configurar pipeline de CI para build, lint e testes.
4. Publicar checklist de setup para novos membros.

## Criterios de Aceite
- Novo dev sobe o ambiente em menos de 30 minutos.
- CI executa automaticamente em push e pull request.
- Build e testes iniciais passam em ambiente limpo.

## Mapeamento com PRD
- NFR1 (Performance, base para cache).
- NFR3 (Padrao de manutencao e escala).
