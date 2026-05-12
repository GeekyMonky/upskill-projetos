# WebAPIVehicles

Web API em ASP.NET Core (.NET 10) para gerir uma frota de veículos.
Adaptação do exemplo `WebAPI2` fornecido pelo professor.

## Tecnologias

- **C#** / **.NET 10**
- **ASP.NET Core** (Minimal API)
- **SQL Server** + **Microsoft.Data.SqlClient**
- **Swagger** (Swashbuckle.AspNetCore)
- **DALPro** — classe de acesso a dados (incluída em `LibDB/DALPro.cs`)

## Pacotes NuGet utilizados

| Pacote | Versão | Para que serve |
|---|---|---|
| `Microsoft.Data.SqlClient` | 7.0.1 | Ligação ao SQL Server |
| `Swashbuckle.AspNetCore` | 10.1.7 | Swagger / OpenAPI |

> 💡 Os pacotes são restaurados automaticamente pelo Visual Studio ao abrir a solução. Se precisares restaurar manualmente: botão direito na solução → **Restore NuGet Packages**.

## Estrutura do projeto## Como correr

```
WebAPIVehicles/
├── LibDB/                     # DALPro — acesso à base de dados
├── Models/                    # Entidades (Vehicle)
├── DTOs/                      # Data Transfer Objects
├── Repositories/              # Acesso à BD (interface + implementação)
├── Services/                  # Lógica de negócio + transações
├── Program.cs                 # Endpoints, DI, Swagger
└── appsettings.json           # Connection string
```

### 1. Criar a base de dados

1. Abrir o ficheiro `database.sql` no SQL Server Management Studio
2. Ligar a uma instância local de SQL Server
3. Executar (F5)

Isto cria a base de dados `VehiclesDB` e insere 5 veículos de teste.

### 2. Configurar a connection string (se necessário)

Se a tua instância de SQL Server **não** for `localhost`, edita o ficheiro `WebAPIVehicles/appsettings.json` e ajusta o campo `Server=...`.

### 3. Correr a aplicação

- Abrir `WebAPIVehicles.sln` no Visual Studio 2022/2026
- Carregar em ▶ (run)
- O navegador abre em `https://localhost:7XXX`

### 4. Testar via Swagger

Acrescentar `/swagger` ao URL — abre uma interface para testar todos os endpoints.

## Endpoints

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/vehicles` | Lista todos os veículos |
| `GET` | `/vehicles/{id}` | Obtém um veículo por id |
| `POST` | `/vehicles` | Cria um veículo novo |
| `PUT` | `/vehicles/{id}` | Atualiza um veículo |
| `DELETE` | `/vehicles/{id}` | Apaga um veículo |

## Modelo de dados

Tabela `Vehicles`:

| Coluna | Tipo | Notas |
|---|---|---|
| `VehicleID` | INT | Chave primária, auto-incremento |
| `Brand` | NVARCHAR(50) | Obrigatório |
| `Model` | NVARCHAR(50) | Obrigatório |
| `Year` | INT | Obrigatório |
| `LastInspection` | DATETIME | Opcional |
| `Sold` | BIT | 0 = não vendido, 1 = vendido |
