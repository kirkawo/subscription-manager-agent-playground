# subscription-manager-agent-playground

A minimal ASP.NET Core Web API for managing customers and subscriptions.  
Built as a **local playground for testing AI coding agents** (Ollama, Cline and similar tools).

## Purpose

This project was created to evaluate and compare how different local LLM-based coding agents (e.g. `qwen3-coder`, `deepseek-coder`, etc.) handle real-world .NET backend tasks:
- generating boilerplate code,
- writing unit tests,
- refactoring services and repositories,
- working with EF Core migrations.

## Tech Stack

- **.NET 10** — Minimal API
- **Entity Framework Core** + SQLite
- **AutoMapper** — DTO/ViewModel mapping
- **FluentValidation** — request validation
- **xUnit** + **Moq** — unit testing
- **Swagger / OpenAPI** — API docs

## Project Structure

src/SubscriptionManager/ # Web API project
tests/SubscriptionManager.Tests/ # Unit tests (xUnit + Moq)


## Endpoints

| Method | Route                  | Description              |
|--------|------------------------|--------------------------|
| GET    | /customers             | Get all customers        |
| GET    | /customers/{id}        | Get customer by ID       |
| POST   | /customers             | Create customer          |
| PUT    | /customers/{id}        | Update customer          |
| DELETE | /customers/{id}        | Delete customer          |
| GET    | /subscriptions         | Get all subscriptions    |
| GET    | /subscriptions/{id}    | Get subscription by ID   |
| POST   | /subscriptions         | Create subscription      |
| PUT    | /subscriptions/{id}    | Update subscription      |
| DELETE | /subscriptions/{id}    | Delete subscription      |

## Getting Started

```bash
# Run the API (locally)
dotnet run --project src/SubscriptionManager.Api

# Run tests
dotnet test
```

Swagger UI is available at `https://localhost:{port}/swagger` when running in Development mode.

### Docker

```bash
# Build and start the API container
docker compose up --build

# Stop and remove the container
docker compose down
```

The API is available at `http://localhost:5022` and Swagger at `http://localhost:5022/swagger`.

SQLite data is persisted in a named Docker volume (`subscription-data`) mapped to `/app/Data` inside the container.

## Notes

- SQLite database is created automatically on first run (`subscription.db`), stored in a `Data/` directory.
- `DatabaseProvider` is set to `Sqlite` by default — override via the `DatabaseProvider` env var.
- Project is intentionally kept simple — the focus is on agent testability, not production readiness.
