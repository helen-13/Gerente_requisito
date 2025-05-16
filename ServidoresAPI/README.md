# Sistema de Gerenciamento de Servidores Públicos

## Estrutura do Projeto

```
ServidoresAPI/
├── src/
│   ├── Commands/
│   │   ├── CreateServidorCommand.cs
│   │   ├── UpdateServidorCommand.cs
│   │   └── DeleteServidorCommand.cs
│   ├── Queries/
│   │   └── GetServidoresQuery.cs
│   ├── Handlers/
│   │   ├── CreateServidorHandler.cs
│   │   ├── UpdateServidorHandler.cs
│   │   ├── DeleteServidorHandler.cs
│   │   └── GetServidoresHandler.cs
│   ├── Models/
│   │   ├── Servidor.cs
│   │   ├── Orgao.cs
│   │   └── Lotacao.cs
│   ├── Data/
│   │   └── ApplicationDbContext.cs
│   ├── DTOs/
│   │   └── ServidorDto.cs
│   └── Validators/
│       └── ServidorValidator.cs
└── tests/
    └── ServidoresAPI.Tests/
        ├── Commands/
        └── Queries/

```

## Requisitos

- .NET 8.0+
- Entity Framework Core
- MediatR
- FluentValidation

## Como Executar

1. Clone o repositório
2. Execute `dotnet restore`
3. Execute `dotnet run`

A API estará disponível em `https://localhost:5001`

## Endpoints

### GET /api/servidores
Lista todos os servidores com opções de filtro.

Query Parameters:
- nome
- orgao
- lotacao

### POST /api/servidores
Cadastra um novo servidor.

Payload:
```json
{
    "nome": "string",
    "telefone": "string",
    "email": "string",
    "orgaoId": 0,
    "lotacaoId": 0,
    "sala": "string"
}
```

### PUT /api/servidores/{id}
Atualiza um servidor existente.

### DELETE /api/servidores/{id}
Inativa um servidor (exclusão lógica).

## Testes

Execute `dotnet test` para rodar os testes unitários. 