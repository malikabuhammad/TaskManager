# TaskManager

Task management API built with .NET 10, EF Core (InMemory) and JWT auth. There are two roles, Admin and User. Admins manage everything, a normal user can only see the tasks assigned to him and update their status.

## Running it

Needs the .NET 10 SDK.

```
dotnet run --project backend/TaskManager.API
```

The browser opens on Scalar (`http://localhost:5159/scalar/v1`), swagger is also there on `/swagger`. The db is in-memory and seeded on startup so you can login directly:

| | email | password |
|---|---|---|
| Admin | admin@gmail.com | 200300123 |
| User | user@gmail.com | 200300123 |

Login through `POST /api/auth/login`, take the token from the response and send it as `Authorization: Bearer <token>`. Tokens last 3 hours.

## Endpoints

```
POST   /api/auth/login              login

POST   /api/users                   admin
GET    /api/users                   admin
GET    /api/users/{id}              admin or the user himself
PUT    /api/users/{id}              admin or the user himself
DELETE /api/users/{id}              admin

POST   /api/tasks                   admin
GET    /api/tasks                   admin
GET    /api/tasks/mytasks           tasks assigned to the logged in user
GET    /api/tasks/{id}              admin or the assigned user
PUT    /api/tasks/{id}              admin
PUT    /api/tasks/{id}/status       admin or the assigned user
DELETE /api/tasks/{id}              admin
```

## Structure

```
backend/
  TaskManager.Domain           entities + result/error types
  TaskManager.Application      services, dtos, repository interfaces
  TaskManager.Infrastructure   EF Core, repositories, unit of work, seeder
  TaskManager.API              controllers, middleware, Program.cs
  TaskManager.Tests            xunit tests
```

## Notes

- Roles are enforced by the auth middleware + `[Authorize]` attributes, ownership checks (is this task actually yours) are done in the services.
- Serilog writes to the console and to `backend/TaskManager.API/logs/`, errors go to a separate file. Every request gets an `X-Correlation-Id` you can find in the logs.
- Tests: `dotnet test`
- CI runs build + tests on every push (github actions).
