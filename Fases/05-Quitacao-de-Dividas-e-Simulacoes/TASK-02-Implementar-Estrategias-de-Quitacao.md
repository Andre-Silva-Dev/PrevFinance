# TASK-02 - Implementar Estrategias de Quitacao

## Objetivo
Implementar ordenacao e simulacao de pagamentos por estrategia financeira.

## Entregaveis
- Estrategia Avalanche (maior juros primeiro).
- Estrategia Bola de Neve (menor saldo primeiro).
- Comparativo de prazo e custo total entre estrategias.

## Passos Tecnicos
1. Criar algoritmo de priorizacao para cada estrategia.
2. Simular amortizacao mes a mes com base em capacidade de pagamento.
3. Expor resultado detalhado por parcela e divida.
4. Definir Avalanche como default e permitir troca.

## Criterios de Aceite
- Resultado muda ao alternar estrategia.
- Avalanche vem selecionada por padrao.
- Sistema informa data prevista de quitacao e custo total.

## Mapeamento com PRD
- FR5 (Simulador de quitacao).
- Business Rule 3 (Avalanche padrao, com troca para Bola de Neve).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
