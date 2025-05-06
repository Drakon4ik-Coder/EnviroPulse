# EnviroPulse

[![CI](https://img.shields.io/badge/CI-configured-success)](.github/workflows/maui-dotnet.yml)
![Version](https://img.shields.io/badge/version-1.0-blue)
[![License](https://img.shields.io/badge/license-MIT-green)](EnviroPulse/LICENSE)

EnviroPulse is a .NET MAUI sensor operations app for managing a distributed environmental monitoring network. The repository combines a cross-platform client, a MySQL-backed domain model, an EF Core migrations tool, and an xUnit test suite in one workspace.

## What The Project Does

EnviroPulse helps operators monitor environmental sensors, maintain their configuration, and manage the data and permissions around them.

Core workflows in the current codebase:

- User authentication with persistent sessions, password hashing, and registration.
- Role-based administration for roles, privileges, and user-role assignments.
- Sensor management for configuration, firmware details, status changes, and validation.
- Operational monitoring with sortable incident counts and per-sensor incident drill-down.
- Map-based monitoring with live status pins and warning detection for stale or out-of-threshold readings.
- Sensor locator routing, including optional route building when an `OpenRouteServiceApiKey` is configured.
- Historical data viewing from bundled spreadsheet datasets for air, water, and weather categories.
- Database backup, restore, retention, and scheduled backup configuration.

## Why The Project Is Useful

- It keeps operational, administrative, and data-oriented workflows in one client instead of splitting them across separate tools.
- It is testable: the repository includes unit tests for services, repositories, converters, navigation, and view models.
- It is structured for ongoing development: the app, schema migration utility, SQL seed scripts, and generated documentation live side by side.
- It supports multiple deployment targets through .NET MAUI, with CI already configured for Windows and Android builds in [`.github/workflows/maui-dotnet.yml`](.github/workflows/maui-dotnet.yml).

## Workspace Layout

| Path | Purpose |
| --- | --- |
| [`EnviroPulse/`](EnviroPulse) | Main .NET MAUI application, UI, services, models, and embedded assets |
| [`EnviroPulse/EnviroPulse.sln`](EnviroPulse/EnviroPulse.sln) | Solution file that ties the workspace together |
| [`EnviroPulse/db/`](EnviroPulse/db) | SQL schema and demo seed data scripts |
| [`Migrations/`](Migrations) | EF Core migrations project and migration helper tool |
| [`Tests/`](Tests) | xUnit test project |
| [`EnviroPulse/docs/`](EnviroPulse/docs) | Generated API docs, diagrams, and feature documentation |

## How To Get Started

### Prerequisites

- .NET 8 SDK
- .NET MAUI workloads (`dotnet workload install maui`)
- MySQL 8.x running locally on port `3306`
- Optional: `dotnet-ef` if you want to create or inspect migrations from the command line

### 1. Restore the solution

```bash
dotnet workload restore EnviroPulse/EnviroPulse.sln
dotnet restore EnviroPulse/EnviroPulse.sln
```

### 2. Configure the database connection

Update the connection strings in:

- [`EnviroPulse/appsettings.json`](EnviroPulse/appsettings.json)
- [`Migrations/appsettings.json`](Migrations/appsettings.json)

Both files currently expect a local MySQL instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=sensor_monitoring;User=root;Password=change-me;SslMode=None;"
  }
}
```

### 3. Create the schema and optional demo data

For a fully seeded local environment, run the bundled SQL scripts:

```bash
mysql -u root -p < EnviroPulse/db/db.sql
mysql -u root -p < EnviroPulse/db/data.sql
```

Use the `Migrations` project when you are changing the EF Core model and want to apply tracked migrations instead of raw SQL:

```bash
dotnet run --project Migrations/Migrations.csproj
```

Migration-specific commands are documented in [`Migrations/README.md`](Migrations/README.md).

### 4. Run the application

For local Windows development:

```bash
dotnet run --project EnviroPulse/EnviroPulse.csproj --framework net8.0-windows10.0.19041.0
```

Other targets already defined in the project file:

- `net8.0-android` on Windows or macOS
- `net8.0-ios` and `net8.0-maccatalyst` on macOS

The plain `net8.0` target is useful for shared-code builds and test runs, but it is not the primary interactive MAUI app target.

### 5. Run the test suite

```bash
dotnet test Tests/Tests.csproj --framework net8.0
```

### 6. Try a seeded login

If you loaded [`EnviroPulse/db/data.sql`](EnviroPulse/db/data.sql), you can sign in with one of the seeded accounts:

```text
Email: admin@example.com
Password: admin123
```

After logging in, a typical first pass is:

1. Open **Sensors** to review configuration and operational status.
2. Open **Data** to inspect the status map and historical datasets.
3. Open **Administration** with an admin account to manage roles and user assignments.

## Where To Get Help

- Start with the generated API docs in [`EnviroPulse/docs/html/index.html`](EnviroPulse/docs/html/index.html) for code-level reference.
- Use [`Migrations/README.md`](Migrations/README.md) for EF Core migration tasks.
- Review feature diagrams in [`EnviroPulse/docs/sensor-locator/`](EnviroPulse/docs/sensor-locator) and [`EnviroPulse/docs/uml/`](EnviroPulse/docs/uml) when changing map or workflow behavior.
- Check the CI workflow in [`.github/workflows/maui-dotnet.yml`](.github/workflows/maui-dotnet.yml) to see how the project is built and tested in automation.
- If you cloned this from a hosted Git repository, use that host's issue tracker and pull requests for support and change discussion.

## Who Maintains And Contributes

The repository currently names **Oleksandr Beichuk** in the MIT license file, which makes him the clearest documented maintainer in-tree.

Recent contributors visible in Git history include:

- Alex180504
- Illia
- Denis Skira
- Oleksandr Beichuk
- Stepan Demianenko
- Drakon4ik-Coder

For contribution expectations, branch hygiene, and pre-PR checks, see [`CONTRIBUTING.md`](CONTRIBUTING.md).
