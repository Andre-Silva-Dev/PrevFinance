# Guia de Identidade Frontend do PrevFinance

## 1. Objetivo do guia

Este documento define o padrão visual e de movimento do frontend do PrevFinance para garantir consistência entre telas, reduzir retrabalho e aumentar percepção de qualidade do produto.

Use este guia sempre que criar ou revisar páginas, componentes e fluxos de navegação.

## 2. Princípios de design

- Clareza primeiro: o usuário precisa entender rapidamente o que fazer em cada tela.
- Destaque com equilíbrio: usar contraste e animação para orientar, não para distrair.
- Consistência modular: novas telas devem reaproveitar os mesmos blocos visuais.
- Movimento com intenção: animações devem comunicar mudança de estado ou foco.
- Acessibilidade como padrão: respeitar redução de movimento e legibilidade.

## 3. Fontes e tipografia

### 3.1 Fontes padrão

Definidas em src/frontend/src/styles.scss:

- Títulos e botões: Poppins (`--font-heading`)
- Textos corridos: Monsterart com fallback em Montserrat (`--font-body`)

```scss
--font-heading: 'Poppins', 'Segoe UI', sans-serif;
--font-body: 'Monsterart', 'Montserrat', 'Segoe UI', sans-serif;
```

### 3.2 Quando usar cada fonte

- Use Poppins em:
  - Títulos (`h1` a `h6`)
  - Botões de ação
  - Rótulos de destaque (ex.: pills)
- Use Monsterart/Montserrat em:
  - Parágrafos
  - Textos auxiliares
  - Labels de formulário
  - Conteúdo de cards

### 3.3 Por que essa escolha

- Poppins dá presença visual e melhora escaneabilidade dos pontos de ação.
- Monsterart/Montserrat mantém leitura confortável em blocos maiores.
- A combinação sustenta um visual moderno sem comprometer clareza.

## 4. Paleta de cores e tokens

Os tokens globais ficam em src/frontend/src/styles.scss (bloco `:root`).

### 4.1 Cores principais

- Fundo base: `--color-bg` e `--color-bg-soft`
- Primária da marca: `--color-primary` / `--color-primary-strong`
- Acento de ação: `--color-accent` / `--color-accent-soft`
- Apoio quente (destaques): `--color-warm`
- Texto: `--color-text`, `--color-text-soft`, `--color-text-muted`
- Superfície e borda: `--color-panel`, `--color-panel-strong`, `--color-border`

### 4.2 Regras de uso

- Ações principais: priorizar `--color-accent`.
- Superfícies de conteúdo: gradientes escuros com borda translúcida.
- Texto principal sempre em alto contraste com o fundo.
- Evitar criar novas cores fixas sem necessidade real.

### 4.3 Por que tokens

- Permitem ajustes globais sem refatoração manual extensa.
- Evitam deriva visual entre páginas desenvolvidas por pessoas diferentes.

## 5. Estrutura de layout padrão

## 5.1 Shell da aplicação

Base em:

- src/frontend/src/app/app.html
- src/frontend/src/app/app.scss

Componentes estruturais:

- `app-shell`: container geral com ambientação de fundo.
- `topbar`: barra de navegação com branding.
- `content`: área central do conteúdo com largura controlada.

### 5.2 Estrutura recomendada para novas páginas

Padrão de página modular:

- Wrapper: `pf-module-page`
- Cabeçalho: `pf-card pf-module-header`
- Blocos de destaque: `pf-feature-grid` + `pf-card pf-feature-item`

Exemplo de esqueleto:

```html
<section class="pf-module-page">
  <article class="pf-card pf-module-header" appRevealOnScroll>
    <span class="pf-pill">Contexto</span>
    <h1>Título da tela</h1>
    <p>Descrição breve e objetiva da proposta da tela.</p>
  </article>

  <article class="pf-feature-grid">
    <section class="pf-card pf-feature-item" appRevealOnScroll [revealDelay]="80">
      <h3>Bloco 1</h3>
      <p>Texto de apoio.</p>
    </section>
  </article>
</section>
```

### 5.3 Quando usar variações

- Tela mais analítica: pode aumentar densidade de cards.
- Tela de onboarding: pode usar hero principal antes do grid.
- Fluxos críticos: reduzir elementos decorativos para foco na tarefa.

## 6. Componentes utilitários de UI

Definidos em src/frontend/src/styles.scss.

### 6.1 `pf-card`

Use para blocos de conteúdo relevantes.

- Quando usar: seções, painéis, agrupamentos de dados.
- Quando evitar: itens muito pequenos e repetitivos (ruído visual).
- Por que: cria hierarquia, profundidade e consistência.

### 6.2 `pf-pill`

Use como etiqueta contextual curta.

- Quando usar: status, categoria, fase do módulo.
- Quando evitar: texto longo ou título principal.
- Por que: orienta leitura escaneável.

### 6.3 `pf-button` e `pf-button secondary`

- `pf-button`: ação primária da tela.
- `pf-button secondary`: ação secundária/complementar.

Regra prática:

- No máximo 1 botão primário por bloco principal.
- Ações concorrentes devem ser secundárias.

### 6.4 `pf-feature-grid`

Grid responsivo para distribuir cards equivalentes.

- Quando usar: listas curtas de funcionalidades, passos, pilares.
- Quando evitar: conteúdos longos heterogêneos.

## 7. Diretrizes de animação e movimento

## 7.1 Objetivo das animações

As animações devem:

- Indicar transição de estado.
- Ajudar na orientação espacial do usuário.
- Melhorar percepção de fluidez e qualidade.

Nunca devem:

- Bloquear interação.
- Causar cansaço visual.
- Competir com o conteúdo principal.

### 7.2 Tipos implementados

1. Transição entre rotas (View Transitions)

- Configuração: src/frontend/src/app/app.config.ts
- Efeitos: `route-fade-out` e `route-fade-in` em src/frontend/src/styles.scss

2. Pulso sutil ao ativar rota

- Lógica: src/frontend/src/app/app.ts
- Estilo: classe `content.route-pulse` em src/frontend/src/app/app.scss

3. Reveal on scroll

- Diretiva: src/frontend/src/app/shared/directives/reveal-on-scroll.directive.ts
- Classes de efeito: `.reveal` e `.reveal-visible` em src/frontend/src/styles.scss

4. Movimento ambiente de fundo

- Keyframes `ambient-drift-left` e `ambient-drift-right` em src/frontend/src/app/app.scss

### 7.3 Como usar reveal em novas telas

1. Importar a diretiva no componente standalone.
2. Aplicar `appRevealOnScroll` nos blocos principais.
3. Escalonar `revealDelay` para efeito em cascata.

Exemplo:

```ts
imports: [RevealOnScrollDirective]
```

```html
<section class="pf-card" appRevealOnScroll [revealDelay]="120"></section>
```

### 7.4 Escala recomendada de delays

- Primeiro bloco: 0ms
- Segundo bloco: 80ms a 120ms
- Terceiro bloco: 160ms a 240ms
- Evitar delays acima de 320ms em telas comuns

### 7.5 Critérios para não poluir

- Máximo de 1 tipo de animação de destaque por região principal.
- Evitar animação contínua em elementos de conteúdo (apenas ambientação de fundo).
- Não animar tudo ao mesmo tempo; usar hierarquia e stagger.

## 8. Acessibilidade e conforto visual

### 8.1 Redução de movimento

Há suporte em `@media (prefers-reduced-motion: reduce)` em:

- src/frontend/src/styles.scss
- src/frontend/src/app/app.scss

Regra obrigatória: qualquer nova animação deve respeitar essa mídia.

### 8.2 Legibilidade

- Priorizar contraste forte entre texto e fundo.
- Limitar largura de parágrafo (ex.: `max-width: 70ch`) para melhor leitura.
- Evitar textos muito claros sobre áreas com brilho intenso.

## 9. Responsividade

Breakpoints base já utilizados:

- 920px: ajustes de topbar e organização geral.
- 768px: ajuste de paddings dos blocos principais.
- 640px: compactação adicional de espaçamentos.

Para novas telas:

- Garantir que grids fechem para 1 coluna em mobile.
- Evitar alturas fixas rígidas em cards de conteúdo.

## 10. Checklist para criar nova tela

1. Estrutura usando `pf-module-page` e `pf-module-header`.
2. Tipografia correta (Poppins em títulos/botões, Monsterart/Montserrat no corpo).
3. Cores via tokens, sem hardcode desnecessário.
4. Ação principal com `pf-button`.
5. Blocos secundários em `pf-card` e `pf-feature-grid`.
6. Reveal on scroll com delays progressivos.
7. Revisão em mobile (920/768/640).
8. Verificação de `prefers-reduced-motion`.
9. Rodar lint e build antes de concluir.

## 11. Processo de revisão visual (PR)

Em toda PR de frontend, verificar:

- Consistência com tokens e classes utilitárias.
- Uso correto de tipografia por hierarquia.
- Animações úteis e discretas.
- Ausência de conflito visual (muitos brilhos, muitos efeitos simultâneos).
- Boa experiência em desktop e mobile.

## 12. Evoluções previstas

- Troca do fallback tipográfico para arquivos oficiais da Monsterart, quando disponibilizados.
- Inclusão de biblioteca oficial de ícones da marca.
- Criação futura de documentação de componentes por domínio (finance, projeção, dívidas, auth).
