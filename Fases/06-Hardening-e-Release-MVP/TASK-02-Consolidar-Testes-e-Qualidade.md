# TASK-02 - Consolidar Testes e Qualidade

## Objetivo
Garantir confiabilidade do sistema e reduzir risco de regressao no MVP.

## Entregaveis
- Suite de testes de unidade para regras criticas.
- Testes de integracao para fluxos principais do PRD.
- Smoke tests end-to-end para jornada MVP.

## Passos Tecnicos
1. Priorizar cobertura de autenticao, parcelamento e projecao.
2. Criar cenarios de integracao para isolamento por UserId.
3. Montar smoke tests para cadastro, conta e dashboard.
4. Integrar execucao no pipeline de CI.

## Criterios de Aceite
- Fluxos MVP passam em execucao automatizada.
- Regras de negocio principais possuem testes de regressao.
- Falhas criticas bloqueiam merge na pipeline.

## Mapeamento com PRD
- Criterios de aceite do MVP.
- FR1, FR2, FR3 e FR4.

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
