# TASK-02 - Integrar OAuth2 e Perfis

## Objetivo
Permitir autenticacao social e consolidar dados de perfil do usuario.

## Entregaveis
- Integracao com pelo menos um provedor OAuth2.
- Vinculacao de conta externa ao usuario interno.
- Tela/API de perfil basico do usuario.

## Passos Tecnicos
1. Configurar fluxo OAuth2 authorization code.
2. Tratar criacao ou vinculacao de conta no primeiro login.
3. Persistir metadados essenciais de perfil.
4. Criar testes de fluxo para login social.

## Criterios de Aceite
- Usuario autentica via OAuth2 e recebe sessao valida.
- Nao existe duplicacao indevida de usuarios por email.
- Perfil pode ser consultado e atualizado no sistema.

## Mapeamento com PRD
- FR1 (Suporte a OAuth2).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
