# TASK-03 - Modelagem Inicial de Dados

## Objetivo
Criar o modelo de dados inicial para usuarios, contas e transacoes com foco em multi-tenancy.

## Entregaveis
- Modelo inicial de tabelas no PostgreSQL.
- Convencao de chaves e auditoria (createdAt, updatedAt).
- Migrations iniciais versionadas.

## Passos Tecnicos
1. Definir entidades base: User, Profile, Account, Transaction.
2. Adicionar coluna UserId nas entidades multi-tenant.
3. Definir indices para consultas frequentes por usuario e periodo.
4. Criar migrations e script de seed tecnico.

## Criterios de Aceite
- Banco sobe com migrations aplicadas sem ajustes manuais.
- Entidades base persistem e consultam com integridade referencial.
- Estrutura pronta para evoluir parcelamentos e projecoes.

## Mapeamento com PRD
- FR1 (Isolamento por UserId).
- FR2 (Contas e saldos).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
