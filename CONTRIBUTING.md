# Contributing

## Development Setup

1. Install the .NET 8 SDK and .NET MAUI workloads.
2. Restore the workspace:

```bash
dotnet workload restore EnviroPulse/EnviroPulse.sln
dotnet restore EnviroPulse/EnviroPulse.sln
```

3. Configure local MySQL connection strings in [`EnviroPulse/appsettings.json`](EnviroPulse/appsettings.json) and [`Migrations/appsettings.json`](Migrations/appsettings.json).
4. Initialize the database with [`EnviroPulse/db/db.sql`](EnviroPulse/db/db.sql) and, if you need demo data, [`EnviroPulse/db/data.sql`](EnviroPulse/db/data.sql).

## Before You Open A Pull Request

- Run the automated tests:

```bash
dotnet test Tests/Tests.csproj --framework net8.0
```

- Keep changes focused and describe the user-visible impact clearly.
- Add or update tests when you change business logic, navigation, repositories, or view models.
- If you change the data model, update the EF Core migrations in [`Migrations/`](Migrations) and keep the SQL scripts in [`EnviroPulse/db/`](EnviroPulse/db) aligned.
- Update the relevant docs under [`EnviroPulse/docs/`](EnviroPulse/docs) when a workflow or architecture diagram changes.

## Pull Request Notes

- Include screenshots for UI changes when the layout or behavior is visible.
- Mention any manual setup needed to test the change locally.
- Call out configuration changes, especially database settings or new external service keys.
