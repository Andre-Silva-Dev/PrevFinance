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
