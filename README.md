# Task-Management

A simple .NET 9 Web API to manage tasks with CRUD operations. Designed to showcase C#/.NET Core skills and best practices.

## Features
- CRUD endpoints for tasks (Title, Description, Status, Priority)
- SQLite database with Microsoft EF Core Migrations

## Technologies
- .NET 9 SDK
- SQLite

## Run the API
```sh
cd TaskManagerApi
dotnet run
```

Default URL: http://localhost:5232

Swagger UI (for manual interactive testing): http://localhost:5232/swagger


## Database
- SQLite by default
- EF Core migrations:
```sh
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Testing
- Framework used: xUnit
- Unit and integration tests at TaskManagerApi.Tests
- Run tests:
```sh
cd TaskManagerApi.Tests
dotnet test
```
- Run individual tests:
```sh
dotnet test --filter "Test_Name"
```
