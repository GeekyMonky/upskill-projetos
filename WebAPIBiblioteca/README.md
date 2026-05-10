# WebAPIBiblioteca

Web API em ASP.NET Core (.NET 10) para gerir uma biblioteca distribuída por vários núcleos.

Adaptação do exemplo `WebAPI5` fornecido pelo professor, com autenticação JWT, logging estruturado e suporte a CORS.

## Tecnologias

- **C#** / **.NET 10**
- **ASP.NET Core** (Minimal API)
- **SQL Server** + **Microsoft.Data.SqlClient**
- **Swagger** (Swashbuckle.AspNetCore) com suporte a JWT
- **JWT Bearer** authentication
- **Serilog** (Console + File)
- **CORS** habilitado
- **DALPro** — classe de acesso a dados (incluída em `LibDB/DALPro.cs`)

## Estrutura

WebAPIBiblioteca/
├── LibDB/                    # DALPro
├── Models/                   # Entidades (7)
├── DTOs/                     # Read + Create DTOs (16)
├── Repositories/             # Interface + impl (14)
├── Services/                 # Interface + impl + AuthService + DevService (16)
├── Program.cs                # Endpoints, DI, Auth, Swagger, Serilog
└── appsettings.json          # Connection string + chave JWT

## Modelo de dados

7 tabelas:

| Tabela | Descrição |
|---|---|
| `Tema` | Temas das obras (ex: Ficção, História) |
| `Nucleo` | Núcleos da biblioteca (Lisboa, Porto, etc.) |
| `Leitor` | Utilizadores registados |
| `Obra` | Obras catalogadas (referencia `Tema`) |
| `Exemplar` | Cópias físicas de obras (referencia `Obra` e `Nucleo`) |
| `Requisicao` | Empréstimos feitos por leitores |
| `ExemplarRequisicao` | Tabela de ligação (chave composta) — relaciona exemplares com requisições, com data de devolução |

## ⚠️ Nota sobre segurança

Este projeto inclui o `appsettings.json` no repositório, com chave JWT e
connection string, **para facilitar a avaliação do professor** (basta
clonar e correr).

Em ambiente de produção, esta abordagem **não seria recomendada**:

- A chave JWT (`App:JWT:SECRET_KEY`) deveria estar em variáveis de
  ambiente ou num cofre seguro (ex: Azure Key Vault)
- O `appsettings.json` real ficaria fora do repositório, e seria
  versionado apenas um `appsettings.example.json` com valores fictícios
- Credenciais de utilizadores (no exemplo, `admin/123` em `Program.cs`)
  seriam guardadas numa tabela de utilizadores com passwords hashed,
  nunca hardcoded
- A regra geral: **nunca commitar segredos para um repositório,
  especialmente público**.

## Como correr

### 1. Criar a base de dados

1. Abrir `database.sql` no SQL Server Management Studio
2. Ligar a uma instância local de SQL Server
3. Executar (F5)

Cria a base `BibliotecaXPTO` com 5 temas, 4 núcleos, 5 leitores, 10 obras, 20 exemplares, 5 requisições e 7 exemplares-requisição.

### 2. Configurar a connection string (se necessário)

Editar `WebAPIBiblioteca/appsettings.json` e ajustar `Server=...` se a instância de SQL Server não for `localhost`.

### 3. Correr

- Abrir `WebAPIBiblioteca.slnx` no Visual Studio 2022/2026
- Carregar em ▶
- O navegador abre em `https://localhost:7XXX`

### 4. Testar via Swagger

- Acrescentar `/swagger` ao URL
- Endpoints públicos: `GET /` e `POST /login`
- Endpoints protegidos: tudo o resto (precisa de token)

#### Fluxo de teste

1. `POST /login` com body:
```json
   { "username": "admin", "password": "123" }
```
2. Copiar o `token` da resposta
3. Clicar em **Authorize** no Swagger e colar o token
4. Testar os endpoints protegidos

## Endpoints

### Autenticação

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/login` | Devolve um token JWT |

### CRUD por entidade (5 endpoints cada)

`GET`, `GET /{id}`, `POST`, `PUT`, `DELETE` para:
- `/temas`
- `/nucleos`
- `/leitores`
- `/obras`
- `/exemplares`

### Requisições (sem PUT)

| Método | Rota |
|---|---|
| `GET` | `/requisicoes` |
| `GET` | `/requisicoes/{id}` |
| `POST` | `/requisicoes` |
| `DELETE` | `/requisicoes/{id}` |

### Empréstimo de exemplares (relação)

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/requisicoes/{id}/exemplares` | Lista exemplares de uma requisição |
| `GET` | `/exemplares/{id}/requisicoes` | Histórico de requisições de um exemplar |
| `POST` | `/requisicoes/{id}/exemplares` | Adiciona exemplar a uma requisição |
| `PUT` | `/requisicoes/{id}/exemplares/{exId}/devolver` | Marca devolução |
| `DELETE` | `/requisicoes/{id}/exemplares/{exId}` | Remove um exemplar de uma requisição |

### Dev-only (apenas em modo Debug)

| Método | Rota | Descrição |
|---|---|---|
| `GET` | `/getModel` | Gera classes C# a partir do schema da BD |

## Autenticação (resumo)

- Login fixo em `Program.cs`: `admin` / `123`
- Token válido por 2 horas
- Issuer e Audience: `biblioteca`
- Chave secreta em `appsettings.json` → `App:JWT:SECRET_KEY`

## Logs

Serilog escreve simultaneamente para:
- **Consola**
- **Ficheiro**: `logs/log.txt` (rotaciona diariamente)

A pasta `logs/` é criada automaticamente ao correr.