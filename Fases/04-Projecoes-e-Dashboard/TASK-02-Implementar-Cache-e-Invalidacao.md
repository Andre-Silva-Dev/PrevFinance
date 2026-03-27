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
