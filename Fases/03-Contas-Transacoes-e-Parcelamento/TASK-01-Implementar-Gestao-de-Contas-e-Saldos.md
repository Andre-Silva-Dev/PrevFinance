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
