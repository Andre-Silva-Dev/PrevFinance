# TASK-01 - Implementar Cadastro e Login

## Objetivo
Disponibilizar autenticacao principal com email e senha.

## Entregaveis
- Endpoints de cadastro e login.
- Emissao e validacao de token JWT.
- Fluxo de refresh token e logout seguro.

## Passos Tecnicos
1. Criar casos de uso de registro e autenticacao.
2. Armazenar senha com hash forte e salt.
3. Implementar emissao de JWT com claims de usuario.
4. Cobrir erros comuns com respostas padronizadas.

## Criterios de Aceite
- Cadastro de novo usuario funcional.
- Login retorna token valido e expira no tempo esperado.
- Rotas protegidas exigem token valido.

## Mapeamento com PRD
- FR1 (Cadastro/Login).
- NFR2 (Seguranca).
