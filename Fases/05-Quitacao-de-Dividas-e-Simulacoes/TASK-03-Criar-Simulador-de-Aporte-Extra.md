# TASK-03 - Criar Simulador de Aporte Extra

## Objetivo
Permitir que o usuario teste quanto acelerar a quitacao com economia adicional mensal.

## Entregaveis
- Entrada de valor de aporte extra mensal.
- Recalculo da nova data de quitacao final.
- Sugestao de cortes em gastos nao essenciais.

## Passos Tecnicos
1. Adicionar parametro de aporte extra na simulacao.
2. Reexecutar algoritmo de quitacao com nova capacidade de pagamento.
3. Mapear gastos marcados como nao essenciais para sugestao de corte.
4. Exibir impacto percentual em prazo e juros.

## Criterios de Aceite
- Aporte extra altera resultado final de forma consistente.
- Sistema apresenta nova data de quitacao.
- Sugestoes de corte priorizam gastos nao essenciais.

## Mapeamento com PRD
- FR5 (Calculo com aporte extra).
- Business Rule 2 (Essencialidade de gastos).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
