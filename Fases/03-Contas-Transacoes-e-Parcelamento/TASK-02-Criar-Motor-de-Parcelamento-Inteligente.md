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

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
