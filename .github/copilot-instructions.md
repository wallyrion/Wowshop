# GitHub Copilot Instructions for Wowshop

## Project Overview

Wowshop is a .NET 8 minimal API project that demonstrates cloud-native containerization and CI/CD deployment patterns using Docker, Azure App Service, and GitHub Actions.

## Tech Stack

- **Runtime**: .NET 8 (`net8.0`)
- **Framework**: ASP.NET Core Minimal APIs (no controllers, no Startup.cs)
- **Language**: C# with nullable reference types and implicit usings enabled
- **Testing**: xUnit + FluentAssertions + `Microsoft.AspNetCore.Mvc.Testing`
- **Containerization**: Native .NET 8 container support (`dotnet publish /t:PublishContainer`)
- **CI/CD**: GitHub Actions
- **Deployment**: Docker Hub + Azure App Service
- **Load Testing**: k6

## Project Structure

```
Wowshop/
├── Wowshop.API/          # Main API project (minimal APIs in Program.cs)
├── Wowshop.Tests/        # xUnit integration tests
├── .github/
│   └── workflows/        # build.yml (PRs), deploy.yml (main branch)
├── k6.js                 # k6 load testing script
└── Wowshop.sln
```

## Coding Conventions

### Minimal APIs
- Define all endpoints directly in `Program.cs` — do **not** create controllers or use the MVC pattern.
- Keep endpoints concise and functional.
- Read application settings via `IConfiguration` injection where needed.

```csharp
// Good
app.MapGet("/example", (IConfiguration config) => config["SomeSetting"]);

// Avoid
[ApiController]
public class ExampleController : ControllerBase { ... }
```

### C# Style
- Use `var` for local variable declarations where the type is evident.
- Prefer expression-bodied members and lambda endpoints.
- Nullable reference types are enabled — annotate nullability explicitly.
- Implicit usings are enabled — do not add redundant `using` directives for common namespaces.

### Configuration
- Environment-specific settings go in `appsettings.{Environment}.json`.
- Sensitive/runtime values (e.g., `CurrentImageTag`) are injected via environment variables or Azure App Service configuration — never hardcoded.

## Testing Conventions

- Use **xUnit** (`[Fact]`, `[Theory]`) for all tests.
- Use **FluentAssertions** for assertions — avoid raw `Assert.*` calls.
- Use `WebApplicationFactory<Program>` for integration tests; do not spin up external servers.
- The `Wowshop.API` project exposes internals to `Wowshop.Tests` via `InternalsVisibleTo`.

```csharp
// Good
var response = await httpClient.GetAsync("/");
response.StatusCode.Should().Be(HttpStatusCode.OK);

// Avoid
Assert.Equal(HttpStatusCode.OK, response.StatusCode);
```

## Deployment & Health Check Pattern

- The `GET /health` endpoint returns the current image tag from configuration.
- The deploy workflow polls `/health` to verify the correct Docker image is running after deployment.
- When adding new health or diagnostic endpoints, follow this pattern: read a value from `IConfiguration` and return it as a simple JSON object or string.

## What to Avoid

- Do **not** introduce a database, ORM (e.g., Entity Framework), or data persistence layer — this project is intentionally stateless.
- Do **not** add traditional MVC controllers, Razor views, or gRPC services.
- Do **not** introduce complex architectural patterns (CQRS, DDD, mediator) unless explicitly requested.
- Do **not** add a `Dockerfile` — container images are built via `dotnet publish /t:PublishContainer`.
- Do **not** skip or bypass GitHub Actions workflow steps (e.g., tests must pass before deployment).

## CI/CD Guidelines

- **PRs** trigger `build.yml`: restore → build → test.
- **Merges to `main`** trigger `deploy.yml`: build → test → publish container → push to Docker Hub → deploy to Azure → verify health.
- Any new environment secrets must be documented and added to both workflow files consistently.
