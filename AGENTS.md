# AGENTS.md

## Start Here

- Read [README.md](README.md) for local setup, Docker and Aspire startup, and Azure deployment flow.
- Read [CONTRIBUTING.md](CONTRIBUTING.md) before making non-trivial design or dependency changes.
- Read [tests/README.md](tests/README.md) before running functional tests.

## Repository Map

- The Aspire entry point is [src/eShop.AppHost/eShop.AppHost.csproj](src/eShop.AppHost/eShop.AppHost.csproj). Most local end-to-end behavior is orchestrated through AppHost.
- Backend services live under `src/Catalog.API`, `src/Basket.API`, `src/Identity.API`, and `src/Ordering.API`, with supporting projects under `src/EventBus*`, `src/IntegrationEventLogEF`, and `src/Shared`.
- UI projects live under `src/WebApp`, `src/WebAppComponents`, `src/HybridApp`, and `src/ClientApp`.
- Browser end-to-end tests live under [e2e/](e2e/) and use Playwright from [package.json](package.json).

## Build And Test Defaults

- Trust [global.json](global.json) for SDK selection. This checkout pins .NET 10 preview even though [README.md](README.md) still mentions .NET 9.
- For most web and backend changes, mirror [pr-validation.yml](.github/workflows/pr-validation.yml):
  - `dotnet build eShop.Web.slnf`
  - `dotnet test --solution eShop.Web.slnf --no-build --no-progress --output detailed`
- Run the full application locally with `dotnet run --project src/eShop.AppHost/eShop.AppHost.csproj`.
- For MAUI-only work, mirror [pr-validation-maui.yml](.github/workflows/pr-validation-maui.yml):
  - `dotnet build src/ClientApp/ClientApp.csproj`
  - `dotnet test --project tests/ClientApp.UnitTests/ClientApp.UnitTests.csproj --no-progress --output detailed`
- Functional tests and AppHost-backed runs require Docker.

## Conventions

- [Directory.Build.props](Directory.Build.props) sets `TreatWarningsAsErrors=true` and `UseArtifactsOutput=true`; expect build output under `artifacts/`.
- Prefer project-scoped edits and validation over solution-wide changes.
- Preserve the conventions already used by the touched project; do not add new libraries or frameworks unless the task requires them.
- Add or update the nearest unit or functional test for every behavior change.

## Existing Custom Agents

- Use [.github/agents/backend-fixer.agent.md](.github/agents/backend-fixer.agent.md) for changes confined to `Catalog.API`, `Basket.API`, `Ordering.API`, `Identity.API`, or their matching tests.

## Practical Notes

- Prefer `eShop.Web.slnf` for routine web and backend work; the repo root also contains `eShop.slnx`.
- If a change touches `src/ClientApp`, treat it as a separate validation slice because CI runs it on Windows with MAUI workloads.
- If a task depends on Azure deployment behavior, follow the `azd` flow in [README.md](README.md) instead of inventing repo-specific deployment steps.