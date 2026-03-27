# TASK-01 - Implementar Gestao de Contas e Saldos

## Objetivo
Permitir cadastro e manutencao de multiplas contas com saldo atual confiavel.

## Entregaveis
- CRUD de contas (corrente, poupanca, cartao, carteira).
- Recalibracao manual de saldo real.
- Historico de ajustes de saldo.

## Passos Tecnicos
1. Implementar entidade Account com tipo, saldo e metadata.
2. Criar endpoints e validacoes de criacao/edicao.
3. Implementar ajuste de saldo com trilha de auditoria.
4. Criar testes de consistencia para saldo atualizado.

## Criterios de Aceite
- Usuario cadastra mais de uma conta com sucesso.
- Saldo pode ser recalibrado sem corromper historico.
- Consultas de contas retornam dados corretos por usuario.

## Mapeamento com PRD
- FR2 (Gestao de contas e saldos).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
