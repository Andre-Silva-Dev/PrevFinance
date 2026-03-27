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

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
