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
