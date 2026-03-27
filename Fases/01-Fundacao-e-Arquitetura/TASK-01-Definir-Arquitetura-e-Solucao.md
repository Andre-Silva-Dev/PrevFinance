# TASK-01 - Definir Arquitetura e Solucao

## Objetivo
Definir a arquitetura base e a organizacao da solucao para suportar crescimento modular.

## Entregaveis
- Estrutura da solucao backend por camadas (Domain, Application, Infrastructure, Api).
- Estrutura inicial frontend Angular por features.
- Documento curto de convencoes arquiteturais.

## Passos Tecnicos
1. Criar solution e projetos base com referencias entre camadas.
2. Definir fronteiras de modulo para Auth, Finance, Projection e Debt.
3. Configurar DI e padrao de composicao de servicos.
4. Criar diagramas simples de fluxo entre camadas.

## Criterios de Aceite
- Estrutura compila sem warnings criticos.
- Dependencias entre camadas respeitam Clean Architecture.
- Existe guia de padrao arquitetural para o time.

## Mapeamento com PRD
- NFR3 (Arquitetura Modular Monolith).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
