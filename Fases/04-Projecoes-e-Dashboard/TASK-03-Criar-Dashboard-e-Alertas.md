# TASK-03 - Criar Dashboard e Alertas

## Objetivo
Construir interface de previsibilidade com leitura clara de risco financeiro.

## Entregaveis
- Tela de dashboard com grafico de saldo acumulado.
- Lista de contas a pagar e receber no horizonte selecionado.
- Alerta visual para saldo futuro negativo.

## Passos Tecnicos
1. Criar componentes Angular para serie temporal e resumo financeiro.
2. Integrar com API de projecoes e contas.
3. Destacar pontos de quebra onde saldo fica abaixo de zero.
4. Adaptar layout responsivo para desktop e mobile.

## Criterios de Aceite
- Usuario visualiza projecao em grafico e lista em uma unica tela.
- Alertas de risco aparecem de forma imediata.
- Interface funciona de forma responsiva.

## Mapeamento com PRD
- FR4 (Dashboard de previsibilidade).
- NFR4 (UX/UI responsiva).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
