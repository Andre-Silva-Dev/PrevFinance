# TASK-01 - Identificar Dividas com Juros

## Objetivo
Classificar automaticamente compromissos financeiros que representam divida ativa.

## Entregaveis
- Regras para detectar dividas a partir de transacoes e contas.
- Campo de taxa de juros e saldo devedor consolidado.
- Endpoint de listagem de dividas elegiveis para simulacao.

## Passos Tecnicos
1. Definir criterio de identificacao de divida no dominio.
2. Normalizar informacoes de juros para calculo comparavel.
3. Criar agregacao de saldo devedor por contrato/cartao.
4. Cobrir com cenarios de dados reais e sinteticos.

## Criterios de Aceite
- Sistema lista dividas com juros de forma correta.
- Taxas e saldo devedor batem com base de origem.
- Resultado pode ser consumido pelo simulador de quitacao.

## Mapeamento com PRD
- FR5 (Identificacao automatica de dividas).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
