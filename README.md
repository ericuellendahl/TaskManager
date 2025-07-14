## Projeto Criação de Tarefas

### Descrição do projeto

Projeto em atualização contínua, voltado para a modernização e o aperfeiçoamento das práticas de desenvolvimento e integração.

## Tecnologias Utilizadas

- .NET 8
- Entity Framework Core
- Bando InMemory
- Docker
- xUnit (testes)
- Swagger/OpenAPI

---

## Endpoints da API

### Projetos
- `GET /api/projects` - Lista todos os projetos do usuário
- `POST /api/projects` - Cria um novo projeto
- `GET /api/projects/{id}/tasks` - Lista tarefas de um projeto
- `DELETE /api/projects/{id}` - Remove um projeto

### Tarefas
- `POST /api/tasks` - Cria uma nova tarefa
- `PUT /api/tasks/{id}` - Atualiza uma tarefa
- `DELETE /api/tasks/{id}` - Remove uma tarefa
- `POST /api/tasks/{id}/comments` - Adiciona comentário a uma tarefa

### Relatórios
- `GET /api/reports/performance` - Relatório de performance (apenas gerentes)

## Testes

Execute os testes unitários com:

```bash
dotnet test
```

```
✅ Gerar relatório de cobertura de testes
Para visualizar a cobertura de testes em formato HTML, execute o comando abaixo após rodar os testes com cobertura:

reportgenerator -reports:"TaskManager.Tests/TestResults/54d0a41e-a906-413e-9e7e-81abc7265267/coverage.cobertura.xml" -targetdir:"TestResults/Report" -reporttypes:Html
Depois, abra o relatório no navegador com:


start TestResults\Report\index.html

O relatório exibirá a porcentagem de cobertura por arquivo, linha e ramo de código de forma visual e detalhada.

```

O projeto mantém mais de 80% de cobertura de código conforme especificado.



## Bando de dados

O banco de dados está em memória, utilizando o provedor InMemory do Entity Framework Core, fornecido pela Microsoft.




