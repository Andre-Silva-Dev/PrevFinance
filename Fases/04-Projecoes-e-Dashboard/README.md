# Fase 04 - Projecoes e Dashboard

## Objetivo
Entregar previsibilidade financeira com projecao de saldo e visualizacao no dashboard.

## Escopo
- Motor de projecao de fluxo de caixa.
- Cache de projecoes em Redis para performance.
- Dashboard com alerta de risco de saldo negativo.

## Independencia da Fase
Fase validavel via API e tela de dashboard, mesmo sem simulador de quitacao final.

## Criterio de Conclusao
- Projecao de 12 meses funcional por dia.
- Dashboard carrega em menos de 2 segundos com cache aquecido.
- Alertas de saldo negativo aparecem corretamente.
- Todas as funcionalidades da fase possuem testes unitarios e de integracao cobrindo sucesso e erro.

## Tasks
- TASK-01-Implementar-Motor-de-Projecao.md
- TASK-02-Implementar-Cache-e-Invalidacao.md
- TASK-03-Criar-Dashboard-e-Alertas.md
