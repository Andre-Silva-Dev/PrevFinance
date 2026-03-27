# TASK-02 - Implementar Cache e Invalidacao

## Objetivo
Cumprir requisito de performance do dashboard usando cache de projecoes.

## Entregaveis
- Cache Redis para resultados de projecao por usuario.
- Estrategia de invalidacao por evento financeiro relevante.
- Metricas de hit/miss para monitoramento.

## Passos Tecnicos
1. Definir chave de cache por usuario e janela temporal.
2. Configurar TTL e politica de refresh sob demanda.
3. Invalidar cache em criacao/edicao/exclusao de transacoes.
4. Expor metricas para acompanhar desempenho real.

## Criterios de Aceite
- Dashboard atende meta de tempo inferior a 2s em ambiente alvo.
- Mudancas financeiras refletem apos invalidacao.
- Metricas de cache ficam disponiveis para acompanhamento.

## Mapeamento com PRD
- NFR1 (Performance com Redis).

## Regra obrigatoria de testes
- Esta task deve seguir a politica de TDD (Test-Driven Development), com escrita de testes antes da implementacao da funcionalidade.
- Esta task so pode ser concluida com testes unitarios e testes de integracao.
- Os testes devem cobrir todos os casos relevantes da funcionalidade, incluindo cenarios de sucesso e cenarios de erro.
- A entrega deve registrar evidencia de execucao dos testes.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representam o comportamento esperado, incluindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer todos os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes existentes.
