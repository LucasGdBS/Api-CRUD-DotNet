<!-- markdownlint-disable MD029 -->
# CrudSimples utilizando .NET Core

## Requisitos

- .NET Core 3.1
- Visual Studio ou Visual Studio Code
- Entity Framework Core .NET Command-line Tools

## Como rodar o projeto

1. Clone o repositório

2. Abra o projeto no Visual Studio ou na sua IDE de preferencia

3. Rode as migrations para criar o banco sqlite e execute o projeto

```bash
dotnet ef database update
dotnet run
```

4. Acesse a API em [`http://localhost:5229/swagger/index.html/api/`](http://localhost:5229/swagger/index.html)
