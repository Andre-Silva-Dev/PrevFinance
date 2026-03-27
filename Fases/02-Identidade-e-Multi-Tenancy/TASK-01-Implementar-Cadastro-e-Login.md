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

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
