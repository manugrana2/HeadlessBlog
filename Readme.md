**README.md**

# HeadLessBlog 📰

> A clean, headless-first Blog Platform API built with **ASP.NET Core 8.0**, **Clean Architecture**, **CQRS**, **MediatR**, **OneOf Pattern**, **FluentValidation**, and **PostgreSQL**.

---

## 🚀 Tech Stack

| Layer | Technology |
|:--|:--|
| Backend | ASP.NET Core 8.0 (Controllers) |
| ORM | Entity Framework Core (PostgreSQL) |
| Database | PostgreSQL |
| Authentication | JWT Bearer Tokens |
| Validation | FluentValidation |
| Dependency Injection | Built-in .NET DI |
| API Documentation | Swagger / OpenAPI |
| Testing | xUnit + FluentAssertions + Moq |
| Logging | Serilog |
| Containerization | Docker + Docker Compose |

---

## 🏩 Architecture Overview

We follow **Clean Architecture** + **Vertical Slicing**.

```
/src
  /HeadLessBlog.Domain
  /HeadLessBlog.Application
  /HeadLessBlog.Infrastructure
  /HeadLessBlog.WebAPI
/tests
  /HeadLessBlog.UnitTests
```

- Domain: Entities, enums, no dependencies.
- Application: Commands, Queries, Interfaces (CQRS).
- Infrastructure: External concerns (EF Core, Repositories, JWT, Password Hashing).
- WebAPI: Controllers, Validation, Authentication, Authorization.

---

## 📦 Folder Structure (Detailed)

```
/src
  /HeadLessBlog.Domain
    - Entities (User, Post, Comment)
    - Enums (Role)

  /HeadLessBlog.Application
    /Common/Interfaces
    /Users/Commands
    /Posts/Commands
    /Comments/Commands
    /Posts/Queries
    /Comments/Queries

  /HeadLessBlog.Infrastructure
    /Persistence
    /Security

  /HeadLessBlog.WebAPI
    /Controllers
    /Models
    /Validators
    /Middlewares

/tests
  /HeadLessBlog.UnitTests
    /Application
    /WebAPI
```

---

## ✅ Validation Flow

- WebAPI Layer uses **FluentValidation**.
- If valid ➔ Command/Query dispatched via **MediatR**.
- Domain remains 100% validation-agnostic.

---

## 🛡️ Security

- JWT Bearer Authentication.
- Creator-only Policy Authorization.
- Passwords hashed manually with SHA256.

---

## 🐳 Docker & PostgreSQL

Run local environment:

```bash
docker-compose up --build
```

Swagger available at:

```bash
http://localhost:8080/swagger
```

---

## 🧪 Testing

- xUnit + FluentAssertions + Moq.
- All Handlers and Validators tested.

---

## ⚡ Development Scripts

```bash
# Run API
cd src/HeadLessBlog.WebAPI
dotnet run
```

---

# 📜 License

MIT License.

---

# ✨ Acknowledgements

Architecture inspired by:

- Clean Architecture by Uncle Bob.
- Vertical Slice Architecture by Jimmy Bogard.
- CQRS + Mediator Patterns.
- Modern ASP.NET Core Best Practices.



---

**CONTRIBUTING.md**

# Contributing to HeadLessBlog 📰

Thank you for considering contributing to **HeadLessBlog**!  
This document explains the **guidelines and practices** for collaborating on this project.

---

## 🚀 Branch Naming

Please name your branches using this format:

```bash
feature/{short-description}
bugfix/{short-description}
refactor/{short-description}
test/{short-description}
```

Examples:

- `feature/create-user-endpoint`
- `bugfix/fix-jwt-expiration`
- `refactor/move-validation-to-webapi`
- `test/login-command-handler-tests`

---

## 🏩 Working on Features

Follow **Vertical Slicing** approach:

1. Create Request and Validator (in WebAPI).
2. Create Command/Query (in Application).
3. Create Handler (in Application).
4. Use OneOf pattern for results. IRequest<OneOf<TResult, TError>>
5. Update related Controller (WebAPI).
6. Write Unit Tests (Application or WebAPI tests).
7. Update Swagger annotations if needed.

---

## ✅ Pull Request Checklist

- [ ] Slice complete (Command/Query/Handler/Validator).
- [ ] OneOf pattern used for result.
- [ ] Controller endpoint mapped.
- [ ] FluentValidation used.
- [ ] Unit Tests added for:
  - Handlers
  - Validators
- [ ] Code builds and tests pass locally.

---

## 📦 Best Practices

- No direct usage of DbContext outside Infrastructure.
- Keep Domain clean (no EF Core annotations).
- No business logic in Controllers.
- Each endpoint must validate inputs explicitly.
- Always return strongly-typed responses (never `object` or `dynamic`).
- Keep exception handling centralized (Middleware).
- Use paging defaults on list queries.

---

## 📜 Code Style

- Use `init` instead of `set` wherever possible.
- Try to name slices consistently:

```
CreateEntityCommand.cs
CreateEntityCommandHandler.cs
CreateEntityResult.cs
CreateEntityErrorResult.cs
CreateEntityError.cs
```

---

# 🙏 Thank you!

By following these guidelines, you help keep **HeadLessBlog** industrial-grade and scalable for everyone.


# TO DO
- [ ] Create build, publish and test pipelines (Azure DevOps / GitHub Actions).
- [ ] Configure and generate a TypeScript Frontend Client with `swagger tofile` and `openapi-typescript-codegen`.
- [ ] Integrate TypeScript Frontend Client generation into CI pipeline.
- [ ] Publish generated TypeScript client to private/public npm feed (Azure Artifacts / npmjs.com).


