# TASK-01 - Implementar Motor de Projecao

## Objetivo
Calcular saldo futuro com base em receitas e despesas previstas.

## Entregaveis
- Servico de projecao diario para janelas de 6, 12 e 24 meses.
- Formula padrao de saldo acumulado por periodo.
- Contrato de API para consulta de projecao.

## Passos Tecnicos
1. Definir agregacao por dia com saldo acumulado.
2. Aplicar formula do PRD para receitas e despesas previstas.
3. Tratar transacoes recorrentes, parceladas e overdue.
4. Cobrir com testes de unidade e integracao por cenarios.

## Criterios de Aceite
- Resultado da projecao corresponde aos cenarios de teste.
- API retorna serie temporal ordenada e consistente.
- Janelas 6/12/24 meses sao suportadas.

## Mapeamento com PRD
- FR4 (Algoritmo de previsibilidade).
- Business Rule 1 (Overdue no saldo atual).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
