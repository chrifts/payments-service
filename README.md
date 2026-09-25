# Payments

Clean Architecture learning project: `Web` → `Infrastructure` → `Application` → `Domain`.

## Prerequisites
- .NET 10 SDK
- Docker + Docker Compose

## Local setup
1. Copy `.env.example` to `.env` and set a value for `DB_PASSWORD` / `MYSQL_HOST_PORT`.
2. Start MySQL: `docker compose up -d`
3. Set the app's connection string (must match `.env`'s password):
   ```
   dotnet user-secrets set "ConnectionStrings:PaymentsDb" "server=localhost;port=3306;database=payments;user=root;password=<same as .env>" --project Web
   ```
4. Restore local tools (installs `dotnet-ef` from `dotnet-tools.json`): `dotnet tool restore`
5. Apply migrations: `dotnet ef database update --project Infrastructure --startup-project Web`
6. Run the app: `dotnet run --project Web`

## Logging
Serilog, console + file (`Web/logs/`). Level per environment via `appsettings.{Environment}.json`.
