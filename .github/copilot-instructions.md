# GitHub Copilot Instructions for eShop

eShop is a reference .NET eCommerce application built with a services-based architecture orchestrated with .NET Aspire. The main backend services in this fork are Catalog.API, Basket.API, Ordering.API, and Identity.API; Aspire wiring lives in AppHost, but cloud-agent code changes should stay focused on the individual service projects and their tests rather than cross-cutting host/UI work.

## In scope
- Modify only these service projects: `src/Catalog.API/`, `src/Basket.API/`, `src/Ordering.API/`, `src/Identity.API/`.
- Add or update tests only in the corresponding test projects under `tests/`:
  - `tests/Catalog.FunctionalTests/`
  - `tests/Basket.UnitTests/`
  - `tests/Ordering.UnitTests/`
  - `tests/Ordering.FunctionalTests/`
- Keep changes narrowly scoped to the service being worked on.

## Out of scope
- Do not modify `src/WebApp/`, `src/AdminApp/`, `src/eShop.AppHost/`, or `src/Mobile.Bff.Shopping/`.
- Do not modify Blazor-based UI projects or components.
- Do not modify unrelated frontend/mobile/client experiences when asked to work on backend service tasks.

## Build and test
- Restore the solution first: `dotnet restore eShop.sln`
- Build a target service with: `dotnet build src/<service>/<service>.csproj`
- Run service tests with: `dotnet test tests/<service>.UnitTests/`
- If a service uses functional tests, prefer the existing functional test project under `tests/` for end-to-end/API behavior.

## Coding conventions
- Follow the existing validation approach already used by each service; do not introduce a new validation framework or style.
- In `Ordering.API`, preserve the current FluentValidation + MediatR behavior/validator patterns.
- In other services, match existing data-annotation, endpoint, repository, and options patterns already present in that service.
- Follow the existing EF Core query style in the touched project; be consistent with current async LINQ usage, tracking behavior, and repository/query organization.
- Prefer incremental changes in the existing folders and namespaces instead of creating new architectural layers.

## Tests
- Add tests beside the relevant service’s current test suite under `tests/`.
- Put Basket unit tests in `tests/Basket.UnitTests/`.
- Put Ordering unit tests in `tests/Ordering.UnitTests/` and Ordering API/integration scenarios in `tests/Ordering.FunctionalTests/`.
- Put Catalog API behavior tests in `tests/Catalog.FunctionalTests/`.
- Use the repository’s existing test framework and patterns in each test project.

## Do not
- Do not add new NuGet packages unless they are clearly necessary and the justification is stated in the PR/task notes.
- Do not modify shared messaging/infrastructure projects such as `src/EventBus*/` or `src/IntegrationEventLogEF/`.
- Do not refactor across services or shared libraries unless the task explicitly requires it.
- Do not change AppHost orchestration, UI flows, or Blazor code for backend-only tasks.
