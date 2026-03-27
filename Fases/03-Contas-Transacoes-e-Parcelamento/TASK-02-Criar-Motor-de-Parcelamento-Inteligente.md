# TASK-02 - Criar Motor de Parcelamento Inteligente

## Objetivo
Gerar automaticamente transacoes futuras a partir de um parcelamento.

## Entregaveis
- Caso de uso de criacao de parcelamento.
- Geracao de parcelas por frequencia e data de inicio.
- Relacao entre transacao mae e transacoes filhas.

## Passos Tecnicos
1. Modelar entidade Parcelamento e relacao com Transaction.
2. Implementar algoritmo de expansao de parcelas por calendario.
3. Persistir parcelas futuras com status pendente.
4. Criar validacoes de valor total e numero de parcelas.

## Criterios de Aceite
- Parcelamento de exemplo 12x cria 12 transacoes futuras.
- Soma das parcelas bate com valor total definido.
- Parcela respeita periodicidade selecionada.

## Mapeamento com PRD
- FR3 (Geracao automatica de parcelas).
