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

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
