# Project **Agents.md** Guide for **Hola Bebé** (.NET 8 Edition)

This **Agents.md** file provides comprehensive guidance for OpenAI Codex (and any AI agents) working with the **Hola Bebé** solution built on **.NET 8**, **ASP.NET Core MVC/Minimal API**, and **Azure Cosmos DB** using a **Generic Repository + Unit of Work** pattern. It covers solution layout, code conventions, domain concepts, API contracts, admin MVC views, and CI checks so that autonomous contributions stay coherent, safe, and maintainable.

---

## 1 · Solution Structure for OpenAI Codex Navigation

```
/                              – Repository root (managed via **dotnet CLI** & VS Code)
├─ src
│  ├─ HolaBebe.Domain/              – Clean domain models, enums, value objects  ▸ HolaBebe.Domain.csproj
│  ├─ HolaBebe.Application/         – DTOs, CQRS commands/queries, validators, mapping ▸ HolaBebe.Application.csproj
│  ├─ HolaBebe.Infrastructure/      – Cosmos DB context, repositories, UnitOfWork ▸ HolaBebe.Infrastructure.csproj
│  ├─ HolaBebe.Api/                 – **REST endpoints** for mobile/web client (Minimal API) ▸ HolaBebe.Api.csproj
│  ├─ HolaBebe.AdminMvc/            – **ASP.NET Core MVC** back‑office (Razor Views) ▸ HolaBebe.AdminMvc.csproj
│  └─ SharedKernel/                 – Cross‑cutting helpers (Result<T>, Guard, Mediator pipeline) ▸ SharedKernel.csproj
│
└─ tests
   ├─ HolaBebe.UnitTests/           – xUnit tests (domain & application layers) ▸ HolaBebe.UnitTests.csproj
   └─ HolaBebe.IntegrationTests/    – WebApplicationFactory + TestContainers for Cosmos ▸ HolaBebe.IntegrationTests.csproj
```

> **Note:** The project is managed entirely with **VS Code** and the `dotnet` CLI—no Visual Studio `.sln` file is required. Use workspace‑level tasks/launch configurations (`.vscode/`) to build, test, and debug.

HolaBebe.sln            – Top‑level solution │ ├─ /src │  ├─ HolaBebe.Domain          – Clean domain models, enums, value objects │  ├─ HolaBebe.Application     – DTOs, CQRS commands/queries, validators, mapping │  ├─ HolaBebe.Infrastructure  – CosmosDB context, repositories, UnitOfWork │  ├─ HolaBebe.Api             – **REST controllers** for mobile/web client (Minimal API style) │  ├─ HolaBebe.AdminMvc        – **ASP.NET Core MVC** area for back‑office (Razor Views) │  └─ SharedKernel             – Cross‑cutting (Result, Guard, Mediator pipeline) │ └─ /tests ├─ HolaBebe.UnitTests       – xUnit tests (domain & app layer) └─ HolaBebe.IntegrationTests– WebApplicationFactory + TestContainers for Cosmos

````

> **OpenAI Codex** should only modify the relevant project; never commit generated `/bin` or `/obj` folders or secrets.json.

---

## 2 · Coding Conventions for OpenAI Codex

| Aspect | Convention |
|--------|------------|
| Language | **C# 12**, nullable enabled, `file‑scoped` namespaces |
| Style    | `dotnet format` defaults (indent=4 spaces). Follow naming rules in `.editorconfig`. |
| Testing  | **xUnit**; arrange‑act‑assert; fluent assertions (`FluentAssertions`). |
| Comments | XML docs for public APIs; inline comments only for complex logic. |
| Async    | All I/O is `async/await`; repository methods return `Task`/`ValueTask`. |
| DI       | Built‑in Microsoft DI; register via `IServiceCollectionExtensions`. |

### 2.1 · Backend Architectural Guidelines

- **Clean Architecture layers** (Domain → Application → Infrastructure → Presentation).
- **Generic Repository** `IGenericRepository<TEntity>` with CRUD + LINQ provider; **CosmosDbRepository** implements it.
- **UnitOfWork** exposes transactional boundary (`SaveChangesAsync`) even if Cosmos single partition; needed for future DBs.
- **MediatR** for CQRS (optional but recommended) lives in *Application*; controllers call mediator.
- **DTO Mapping** via `Mapster` (lightweight) or `AutoMapper`; mapping profiles in *Application*.
- **Validation** with `FluentValidation`; API filters translate to **422**.
- **Error Handling**: `ProblemDetails` middleware returns RFC 7807 payloads.

### 2.2 · API Project (HolaBebe.Api)

- Minimal API (`app.MapGroup("/api/v1")`), separated per feature folder.
- JWT Bearer authentication: validate token issued by external provider; extract `sub` → `UserId`.
- Produces `application/json` exclusively.

### 2.3 · Admin MVC Project (HolaBebe.AdminMvc)

- Area "Admin" with Razor Pages for CRUD; layout uses Bootstrap 5.
- Authorization policy `RequireAdminRole` (role comes from JWT or cookie auth in future).
- Shared view components live in `/Views/Shared`.

---

## 3 · Domain Model Cheat‑Sheet

All entities inherit `AuditableEntity`:
```csharp
public abstract class AuditableEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; init; } = DateTime.UtcNow;
    public DateTime UpdatedAtUtc { get; private set; } = DateTime.UtcNow;
    public Guid UserId { get; init; }  // FK to auth user
    public void Touch() => UpdatedAtUtc = DateTime.UtcNow;
}
````

| Entity                | Key Properties                                                                                                         |
| --------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| `UserProfile`         | `DisplayName`, `PhotoUrl`, `BirthDate`, `Gender`, `Country`, `Phone`, `Goals`, `Interests`                             |
| `Pregnancy`           | `Current`, `GestationalAgeDays`, `ConceptionDate`, `DueDate`, `LastMenstruationDate`, `EstimationMethod`, `WeekNumber` |
| `FruitSize` (catalog) | `Week`, `FruitName`, `LengthMm`, `WeightG`, `ImageUrl`                                                                 |
| `WeeklyContent`       | `Week`, `Category`, `Title`, `Html`, `VideoUrl`, `Author`, `Locale`                                                    |
| `Article`             | `Slug`, `Title`, `Excerpt`, `Html`, `CoverImageUrl`, `Tags`, `PublishedAt`, `AuthorId`                                 |
| `CalendarEvent`       | `Title`, `Description`, `StartUtc`, `EndUtc`, `Type`, `Color`, `PregnancyId`, `Location`                               |
| `TutorialSlide`       | `Order`, `Title`, `Subtitle`, `ImageUrl`                                                                               |

Enums live in `Domain/Enums.cs`.

---

## 4 · Repository & Unit of Work Contracts

```csharp
public interface IGenericRepository<TEntity> where TEntity : AuditableEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct);
    IAsyncEnumerable<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct);
    Task AddAsync(TEntity entity, CancellationToken ct);
    Task UpdateAsync(TEntity entity, CancellationToken ct);
    Task DeleteAsync(TEntity entity, CancellationToken ct);
}

public interface IUnitOfWork
{
    IGenericRepository<UserProfile> UserProfiles { get; }
    IGenericRepository<Pregnancy> Pregnancies { get; }
    // … other aggregates
    Task<int> SaveChangesAsync(CancellationToken ct);
}
```

`CosmosDbUnitOfWork` implements `IUnitOfWork`; uses a single `CosmosClient` injected via `IOptions<CosmosSettings>`.

---

## 5 · REST API Contract (High‑level)

Prefix `/api/v1`. Standard response codes **200/201/204/400/401/403/404/422**.

| Verb      | Route                          | Handler                          | Input                | Output                    |
| --------- | ------------------------------ | -------------------------------- | -------------------- | ------------------------- |
| **GET**   | `/me/profile`                  | `ProfileEndpoints.Get`           | —                    | `UserProfileDto`          |
| **PUT**   | `/me/profile`                  | `ProfileEndpoints.Put`           | `UserProfileDto`     | same                      |
| **POST**  | `/pregnancies`                 | `PregnancyEndpoints.Create`      | `CreatePregnancyCmd` | `PregnancyDto`            |
| **GET**   | `/pregnancies/current`         | `PregnancyEndpoints.GetCurrent`  | —                    | `PregnancyDto`            |
| **PATCH** | `/pregnancies/{id}`            | `PregnancyEndpoints.Update`      | `UpdatePregnancyCmd` | `PregnancyDto`            |
| **GET**   | `/pregnancies/{id}/fruit-size` | `PregnancyEndpoints.FruitSize`   | —                    | `FruitSizeDto`            |
| **GET**   | `/pregnancies/{id}/week/{n}`   | `PregnancyEndpoints.WeekSummary` | —                    | `WeeklySummaryDto`        |
| **GET**   | `/calendar-events`             | `CalendarEndpoints.List`         | query                | `Paged<CalendarEventDto>` |
| **POST**  | `/calendar-events`             | `CalendarEndpoints.Create`       | `CreateEventCmd`     | `CalendarEventDto`        |
| etc…      |                                |                                  |                      |                           |

Full OpenAPI spec lives in `/docs/openapi.yaml`; Codex must keep it updated using `Swashbuckle` annotations.

---

## 6 · Admin MVC Areas

- **Dashboard**: statistics widgets (pregnancies active, articles views).
- **Content** → CRUD for `WeeklyContent`, `FruitSize`, `TutorialSlide` via scaffolding + custom view models.
- **Blog** → manage `Article` (markdown editor).

Authorization via policy `AdminOnly`.

---

## 7 · Programmatic Checks & CI

```bash
# build & lint
dotnet format --verify-no-changes
dotnet build -c Release

# run tests + coverage
dotnet test /p:CollectCoverage=true
```

CI pipeline (GitHub Actions) fails on warnings‑as‑errors or coverage < 90 %.

---

## 8 · Test Strategy

- **Unit**: Domain rules (e.g. gestational age calc) – pure, no Cosmos.
- **Integration**: use `CosmosDbEmulator` container; spin up `WebApplicationFactory<Program>`.
- Factories & builders for entities live in `tests/TestUtilities`.

---

## 9 · Key Algorithms

- **Gestational Age**: If only `DueDate` known: `280 - (DueDate.UtcDate - UtcToday).TotalDays`.
- **WeekNumber**: `Math.Clamp((int)Math.Floor(days / 7) + 1, 1, 42)`.

Unit‑test leap years, timezone edges, and premature scenarios.

---

## 10 · Roadmap Hints

| Epic                     | Note                                                                                       |
| ------------------------ | ------------------------------------------------------------------------------------------ |
| **Store Module**         | Separate bounded context; new `Store` tables, integrate Stripe via `MinimalApiExtensions`. |
| **Notification Service** | BackgroundService + Azure Notification Hubs.                                               |
| **Multi‑tenant**         | Add `TenantId` to `AuditableEntity`; partition key in Cosmos.                              |

Keep the architecture hexagonal/clean to support these evolutions.

---

**Happy coding!** – This .NET‑focused **Agents.md** should answer most questions for autonomous agents contributing to **Hola Bebé**. Ask via PR discussions when in doubt.

