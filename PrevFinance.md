# 📄 Product Requirements Document (PRD) - PrevFinance

**Versão:** 1.0  
**Status:** Planejamento Finalizado  
**Autor:** André Silva (Lead Architect) & Gemini (AI Collaborator)  
**Data:** Março de 2026

---

## 1. Visão Geral e Objetivos
O **PrevFinance** é uma plataforma de inteligência financeira focada em **previsibilidade ativa**. Diferente de gerenciadores de gastos convencionais que focam no passado, o PrevFinance utiliza dados de recorrência, parcelamentos e metas para simular o cenário financeiro do usuário em meses ou anos à frente.

### Objetivos Principais:
* **Previsibilidade:** Antecipar o saldo bancário em janelas de 6, 12 e 24 meses.
* **Gestão de Dívidas:** Criar planos matemáticos de quitação (Métodos Avalanche e Bola de Neve).
* **Educação Financeira Ativa:** Calcular taxas de economia necessárias para atingir objetivos específicos ou sair da insolvência.

---

## 2. Público-Alvo
1.  **O Planejador:** Profissionais que desejam otimizar investimentos e prever o impacto de grandes compras.
2.  **O Recuperador:** Indivíduos endividados que precisam de um caminho visual e matemático para a quitação.
3.  **O Autônomo:** Usuários com renda variável que precisam entender seu "mínimo necessário" de sobrevivência.

---

## 3. Requisitos Funcionais (FR)

### FR1: Autenticação e Multi-Tenancy
* **Cadastro/Login:** Suporte a E-mail/Senha e OAuth2.
* **Isolamento de Dados:** Garantia de que nenhum usuário acesse dados de terceiros. Filtro global por `UserId` em todas as consultas ao banco.

### FR2: Gestão de Contas e Saldos
* O usuário pode cadastrar múltiplas contas (Corrente, Poupança, Cartão de Crédito, Carteira).
* Recalibração de saldo real para ajuste das projeções futuras.

### FR3: Motor de Parcelamento Inteligente (Core)
* **Entrada:** Valor Total, Nº de Parcelas, Data de Início, Frequência e Categoria.
* **Geração Automática:** O sistema deve criar individualmente as parcelas como transações futuras.
* **Edição em Cascata:** Opção de alterar valor/data de uma única parcela ou de todo o restante da série.

### FR4: Dashboard de Previsibilidade
* **Gráfico de Fluxo de Caixa:** Visualização do saldo acumulado dia a dia pelos próximos 12 meses.
* **Algoritmo:** $Saldo_{Hoje} + \sum(Receitas_{Previstas}) - \sum(Despesas_{Previstas})$.
* **Alertas de Risco:** Notificação visual imediata caso o saldo projetado fique negativo em qualquer ponto do futuro.

### FR5: Simulador de Quitação de Dívidas
* Identificação automática de dívidas com juros.
* Cálculo de "Aporte Extra": O usuário define quanto pode poupar a mais e o sistema projeta a nova data de quitação final.

---

## 4. Requisitos Não-Funcionais (NFR)

| ID       | Categoria       | Requisito                                                                     |
| -------- | --------------- | ----------------------------------------------------------------------------- |
| **NFR1** | **Performance** | Carregamento do Dashboard em < 2s via cache de projeções (Redis).             |
| **NFR2** | **Segurança**   | Criptografia de dados sensíveis (descrições) e TLS em trânsito.               |
| **NFR3** | **Arquitetura** | Modular Monolith (Clean Architecture) para facilitar manutenção e escala.     |
| **NFR4** | **UX/UI**       | Interface responsiva baseada em TailwindCSS, focada em visualização de dados. |

---

## 5. Regras de Negócio (Business Rules)

1.  **Tratamento de Atrasos:** Transações pendentes com data de vencimento retroativa são marcadas como `Overdue` e mantidas no cálculo de saldo atual até serem pagas ou canceladas.
2.  **Essencialidade de Gastos:** Gastos marcados como "Não Essenciais" são os primeiros sugeridos para corte em simulações de economia.
3.  **Priorização de Quitação:** Por padrão, o sistema sugere a estratégia **Avalanche** (maiores juros primeiro), permitindo a troca para **Bola de Neve** (menores valores primeiro).

---

## 6. Especificações Técnicas Sugeridas
* **Backend:** ASP.NET Core 10 (C#).
* **Frontend:** Angular 21 (Signals & Standalone Components).
* **Database:** PostgreSQL (Relacional) + Redis (Cache).
* **Jobs:** Hangfire ou Worker Service para processamento assíncrono das projeções.

---

## 7. Critérios de Aceite (MVP)
* [ ] Fluxo completo de autenticação e criação de perfil.
* [ ] Cadastro de conta bancária com saldo inicial.
* [ ] Lançamento de um parcelamento (ex: 12x) com reflexo imediato no gráfico de 12 meses.
* [ ] Visualização de Dashboard com saldo projetado e lista de contas a pagar/receber.