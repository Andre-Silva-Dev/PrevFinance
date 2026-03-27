# TASK-03 - Implementar Edicao Cascata e Regra Overdue

## Objetivo
Permitir ajuste fino de parcelas e aplicar regra de atraso conforme negocio.

## Entregaveis
- Edicao de parcela unica.
- Edicao em cascata para parcelas futuras da mesma serie.
- Marcacao automatica de overdue para itens vencidos pendentes.

## Passos Tecnicos
1. Criar operacao de patch para parcela individual.
2. Criar operacao de atualizacao em lote para restante da serie.
3. Implementar rotina que marca pendencias retroativas como overdue.
4. Criar testes para conflitos de edicao e idempotencia.

## Criterios de Aceite
- Usuario escolhe editar uma parcela ou toda a serie futura.
- Parcelas vencidas nao pagas ficam com status overdue.
- Calculo de saldo atual considera overdue ate quitacao/cancelamento.

## Mapeamento com PRD
- FR3 (Edicao em cascata).
- Business Rule 1 (Tratamento de atrasos).
