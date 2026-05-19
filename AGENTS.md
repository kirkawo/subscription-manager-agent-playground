# Subscription Manager Agent Playground

## Commands

```bash
dotnet run --project src/SubscriptionManager.Api      # start API (port 5022 / 7286)
dotnet test                                            # all tests
dotnet test --filter "FullyQualifiedName~Customer"     # single class
```

Swagger at `/swagger` in Development mode.

## Project layout

| Directory | Purpose |
|---|---|
| `src/SubscriptionManager.Api/` | Web API – HTTP layer, composition root (`Program.cs:72-115`), middleware |
| `src/SubscriptionManager.Application/` | Use-case logic, DTOs/ViewModels, validators, service/repository interfaces, AutoMapper profile |
| `src/SubscriptionManager.Domain/` | Entities (`Customer`, `Subscription`) – no external dependencies |
| `src/SubscriptionManager.Infrastructure/` | EF Core `AppDbContext`, repository implementations |
| `src/SubscriptionManager.Migrations.*/` | Empty placeholders – **not yet wired** |
| `tests/SubscriptionManager.Tests/` | xUnit + Moq unit tests |

## Dependency flow

```
Api → Application, Infrastructure
Application → Domain
Infrastructure → Domain, Application
Domain → (none)
```

## Architecture (clean/layered)

- **Minimal API** – endpoints defined inline in `Program.cs`
- **EF Core** – `AppDbContext` configures Customer↔Subscription one-to-many; unique index on (CustomerId, Name)
- **Database** – defaults to SQLite (`subscription.db`, created on first run via `db.Database.Migrate()`). Set `DatabaseProvider` env var to `"SqlServer"` to switch (launchSettings.json does this automatically)
- **Validation** – FluentValidation validators injected per-endpoint
- **Mapping** – AutoMapper `MappingProfile` in Application layer
- **Error handling** – `GlobalExceptionHandler` middleware catches `NotFoundException` → 404, `ValidationException` → 400, everything else → 500

## Test patterns (xUnit + Moq)

- Use real AutoMapper with `MappingProfile` (not mocks)
- Use `NullLogger<T>.Instance` for `ILogger<T>`
- Mock repositories only (`ICustomerRepository` / `ISubscriptionRepository`)
- Repository `GetByIdAsync` returns null to test not-found paths → expect `NotFoundException`
- Namespace: `SubscriptionManager.Tests.ServicesTests`

## Quirks

- `SubscriptionManager.http` still references `/weatherforecast/` (dead endpoint)
- No `.editorconfig`, `Directory.Build.props`, or `global.json`
- `*.db` is gitignored late in `.gitignore`, but old `subscription.db` was accidentally tracked
- DatabaseProvider defaults to Sqlite in `appsettings.json` but launchSettings.json overrides it to SqlServer
- Migration projects exist on disk but are not wired into the solution yet
