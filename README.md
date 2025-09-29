# Task-Management

A simple .NET 9 Web API to manage tasks with CRUD operations. Designed to showcase C#/.NET Core skills and best practices.

## Features
- CRUD endpoints for tasks (Title, Description, Status, Priority)
- SQLite database with Microsoft EF Core Migrations
- Returns meaningful HTTP status codes (400, 404, 201, etc.)
- Unit & integration testing:
    - Tests for validation rules (required fields, title length, priority invariants)
    - Tests for API endpoints with in-memory database
- CI/CD: GitHub Actions pipeline runs build + tests on each commit

## Technologies
- .NET 9 SDK
- SQLite
- xUnit

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

## CI/CD Workflow

This project includes a GitHub Actions workflow that runs automatically on every push to main:
- Checkout, pulls the repository code
- Setup .NET 9 SDK
- Restore dependencies, dotnet restore
- Build, dotnet build
- Run tests, dotnet test

View results in the Actions tab on GitHub
