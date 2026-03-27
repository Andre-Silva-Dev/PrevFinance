# TASK-03 - Aplicar Isolamento Global por UserId

## Objetivo
Garantir segregacao completa de dados por tenant em toda a aplicacao.

## Entregaveis
- Filtro global por UserId em repositorios e consultas.
- Guardas de autorizacao para acesso por recurso.
- Testes de seguranca para evitar vazamento entre usuarios.

## Passos Tecnicos
1. Propagar contexto do usuario autenticado para camada de dados.
2. Aplicar filtros automaticos em consultas por tenant.
3. Validar ownership em operacoes de update e delete.
4. Criar testes de integracao para casos de acesso cruzado.

## Criterios de Aceite
- Nenhum endpoint retorna dados de outro usuario.
- Tentativas de acesso externo sao bloqueadas com status adequado.
- Logs de auditoria registram negacoes relevantes.

## Mapeamento com PRD
- FR1 (Isolamento de dados por UserId).
- NFR2 (Seguranca).
