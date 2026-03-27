# Padronizacao do Fluxo de Desenvolvimento

## Objetivo
Definir um fluxo unico de desenvolvimento para as tasks do PrevFinance, garantindo previsibilidade, qualidade e consistencia de entrega.

## Escopo
Este fluxo vale para qualquer task de qualquer fase do projeto.

## Politica Obrigatoria de Testes
- Toda funcionalidade implementada deve possuir testes unitarios e testes de integracao.
- A cobertura deve contemplar todos os cenarios possiveis da funcionalidade, incluindo sucesso e erro.
- Sem evidencia de execucao e aprovacao desses testes, a task nao pode ser considerada pronta para merge.

## Politica Obrigatoria de TDD
- Todo desenvolvimento deve seguir TDD (Test-Driven Development).
- Nao e permitido iniciar implementacao de funcionalidade sem antes definir e escrever os testes correspondentes.
- A implementacao so pode avancar para merge apos os testes de unidade e integracao estarem aprovados.

## Ciclo TDD (Padrao)
1. Red: escrever primeiro os testes unitarios e de integracao que representem o comportamento esperado, cobrindo sucesso e erro.
2. Green: implementar o minimo necessario para fazer os testes passarem.
3. Refactor: melhorar design, legibilidade e duplicacoes sem quebrar os testes.

## Convencao de Branch
- Branch base de integracao: `dev`
- Branch de trabalho da task/fase: `fase/<fase>-task-<id>-<descricao-curta>`

Exemplos:
- `fase/03-task-01-gestao-contas`
- `fase/04-task-02-cache-projecao`

## Fluxo Obrigatorio

### 1. Criar branch da task
Partindo sempre da branch `dev` atualizada.

```bash
git checkout dev
git pull origin dev
git checkout -b fase/<fase>-task-<id>-<descricao-curta>
```

### 2. Desenvolver a task completa
- Implementar toda a task planejada.
- Evitar commits parciais sem valor funcional.
- Manter aderencia aos criterios de aceite da task.

### 3. Validar build, subida da aplicacao e testes
Antes de commitar, validar obrigatoriamente:
- Build do backend
- Build do frontend
- Subida da aplicacao
- Testes unitarios cobrindo todos os cenarios relevantes de sucesso e erro
- Testes de integracao cobrindo todos os cenarios relevantes de sucesso e erro

Comandos de referencia (ajustar conforme scripts oficiais do repositorio):

```bash
# Backend
dotnet build
dotnet test

# Frontend
npm install
npm run build
npm test

# Integracao (exemplo)
dotnet test --filter Category=Integration
```

Checklist minimo de validacao:
- Build sem erro
- Aplicacao sobe corretamente
- Testes unitarios passando com cobertura dos cenarios de sucesso e erro da funcionalidade
- Testes de integracao passando com cobertura dos cenarios de sucesso e erro da funcionalidade

### 4. Commitar o codigo implementado
Commit deve representar a entrega funcional da task.

```bash
git add .
git commit -m "feat(fase-<fase>): conclui task <id> - <descricao>"
```

### 5. Publicar o commit
Publicar a branch no repositorio remoto.

```bash
git push -u origin fase/<fase>-task-<id>-<descricao-curta>
```

### 6. Fazer merge da branch da fase na branch dev
Merge deve ocorrer com revisao (PR) e validacoes de CI aprovadas.

Opcoes recomendadas:
- Via Pull Request: `fase/...` -> `dev`
- Via linha de comando, apos aprovacao:

```bash
git checkout dev
git pull origin dev
git merge --no-ff fase/<fase>-task-<id>-<descricao-curta>
git push origin dev
```

### 7. Deletar a branch da fase local e remota
Depois do merge em `dev`, remover a branch para manter higiene do repositorio.

```bash
# Local
git branch -d fase/<fase>-task-<id>-<descricao-curta>

# Remota
git push origin --delete fase/<fase>-task-<id>-<descricao-curta>
```

## Criterio de Pronto da Task
Uma task so e considerada concluida quando:
1. Implementacao esta completa.
2. Build passou e testes unitarios e de integracao cobrem sucesso e erro e estao aprovados.
3. Codigo foi commitado e publicado.
4. Merge em `dev` foi realizado.
5. Branch local e remota foram removidas.

## Observacoes de Governanca
- Nao pular etapas de teste.
- Nao fazer merge direto em `main` para desenvolvimento de task.
- Em caso de falha de build/teste, corrigir antes de commit final.
- Em caso de lacuna de cobertura em sucesso/erro, complementar testes antes de merge.
