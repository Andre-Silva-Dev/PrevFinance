# Fase 03 - Contas, Transacoes e Parcelamento

## Objetivo
Entregar o nucleo operacional financeiro de contas, lancamentos e parcelamentos.

## Escopo
- Gestao de contas e saldos reais.
- Registro de receitas e despesas futuras.
- Motor inteligente de parcelamento com edicao em cascata.

## Independencia da Fase
Esta fase pode ser usada de forma isolada por API para validar motor financeiro sem dashboard completo.

## Criterio de Conclusao
- Usuario cria contas e ajusta saldo.
- Parcelamento gera transacoes futuras automaticamente.
- Edicao de parcela individual e em serie funciona conforme regra.
- Todas as funcionalidades da fase possuem testes unitarios e de integracao cobrindo sucesso e erro.

## Tasks
- TASK-01-Implementar-Gestao-de-Contas-e-Saldos.md
- TASK-02-Criar-Motor-de-Parcelamento-Inteligente.md
- TASK-03-Implementar-Edicao-Cascata-e-Regra-Overdue.md
