# GitHub Copilot Instructions for eShop

eShop is a reference .NET eCommerce application built with a services-based architecture orchestrated by .NET Aspire. For cloud-agent tasks, focus on the backend service code in Catalog.API, Basket.API, Ordering.API, and Identity.API and their service-specific tests under `tests/`; avoid host, UI, and Blazor work unless a task explicitly says otherwise.

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
- Do not modify anything Blazor.
- Do not modify unrelated frontend, mobile, or client experience projects for backend service tasks.

## Build and test
- Restore the solution first: `dotnet restore eShop.sln`
- Build a target service with: `dotnet build src/<service>/<service>.csproj`
- Run unit tests with: `dotnet test tests/<service>.UnitTests/`
- For Catalog and Ordering API end-to-end behavior, use the existing functional test projects under `tests/`.

## Coding conventions
- Follow the validation approach already used by the touched service; do not introduce a new validation framework or style.
- In `src/Ordering.API/`, preserve the existing FluentValidation + MediatR behavior and validator patterns.
- In the other in-scope services, match existing data-annotation, endpoint, repository, and options patterns already present in that service.
- Follow the existing EF Core query style in the touched project; stay consistent with current async LINQ usage, tracking behavior, and query/repository organization.
- Prefer incremental changes in existing folders and namespaces instead of introducing new architectural layers.

## Tests
- Add tests beside the relevant service’s current test suite under `tests/`.
- Put Basket unit tests in `tests/Basket.UnitTests/`.
- Put Ordering unit tests in `tests/Ordering.UnitTests/` and Ordering API integration scenarios in `tests/Ordering.FunctionalTests/`.
- Put Catalog API behavior tests in `tests/Catalog.FunctionalTests/`.
- Follow the repository’s current test project patterns. Do not assume xUnit if the existing project uses a different test SDK.

## Do not
- Do not add new NuGet packages unless they are clearly necessary and the justification is stated in the task or PR notes.
- Do not modify shared messaging or infrastructure projects such as `src/EventBus*/` or `src/IntegrationEventLogEF/`.
- Do not refactor across services or shared libraries unless the task explicitly requires it.
- Do not change AppHost orchestration, UI flows, or Blazor code for backend-only tasks.
