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
