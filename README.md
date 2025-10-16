# 🧠 BlogPlatform.Api — .NET 8 REST API

[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-InMemory-success)](https://learn.microsoft.com/ef/)
[![JWT Bearer](https://img.shields.io/badge/Auth-JWT-orange)](https://jwt.io/)
[![Tests](https://img.shields.io/badge/Tests-xUnit-green)](https://xunit.net/)
[![License](https://img.shields.io/badge/license-MIT-lightgrey)](LICENSE)

> Modern, clean, and testable RESTful API built with **.NET 8**, **EF Core InMemory**, and **JWT Authentication**, following a **three-layer architecture**.

---

## 📋 Overview
**BlogPlatform.Api** is a RESTful API developed in **.NET 8**, using **Entity Framework Core (InMemory)** for data persistence, **JWT Bearer authentication**, and **Swagger** for API documentation.  
The project follows a **clean, layered architecture** (Controllers → Services → Data) with a **global error-handling middleware** and a unified `Result<T>` response model.

It includes a **unit test project (xUnit)** and a ready-to-use `.gitignore`.

---

## 🧱 Architecture
```
Controllers  →  Services  →  Data (Repositories)
```

### **Controllers**
- Handle requests and responses only (no logic).
- Return standardized `Result<T>` objects.
- Use `[Authorize]` for protected routes and `[AllowAnonymous]` for public ones.

### **Services**
- Contain business logic, validation, and orchestration.
- Interact with repositories.
- Return `Result<T>` with data or validation errors.

### **Data Layer**
- Uses **EF Core InMemory**.
- Contains `AppDbContext` and `BlogPostRepository`.

---

## ⚙️ Features
| Feature | Description |
|----------|-------------|
| ✅ RESTful CRUD | Create, list, and detail blog posts with comments |
| 💬 Comments | Add comments to existing posts |
| 🔐 JWT Authentication | Protects write endpoints |
| 📘 Swagger | Auto API docs at root `/` in development |
| 🧱 Global Middleware | Captures exceptions and wraps them in `Result<T>` |
| 🧪 Unit Tests | xUnit tests for business logic |
| 🧹 Clean Architecture | Three independent layers |

---

## 🔐 Authentication (JWT)
Configuration in `appsettings.Development.json`:
```json
"Jwt": {
  "Issuer": "BlogPlatform",
  "Audience": "BlogPlatformAudience",
  "Key": "super-secret-dev-key-change-me-1234567890"
}
```

### Request token
```bash
curl -X POST http://localhost:5000/api/auth/token   -H "Content-Type: application/json"   -d '{"username":"juan","password":"dev"}'
```

### Use token
```bash
curl -H "Authorization: Bearer <JWT>" http://localhost:5000/api/posts
```

---

## 🧩 Endpoints
| Method | Route | Auth | Description |
|---------|--------|------|-------------|
| `POST` | `/api/auth/token` | ❌ | Get JWT token |
| `GET` | `/api/posts` | ❌ | List all posts |
| `GET` | `/api/posts/{id}` | ❌ | Get post details |
| `POST` | `/api/posts` | ✅ | Create a new post |
| `POST` | `/api/posts/{id}/comments` | ✅ | Add comment |

---

## 🧪 Unit Tests
- Framework: **xUnit**
- SDK: **Microsoft.NET.Test.Sdk 17.11.1**
- Scope: **Service layer**
- Coverage:
  - Post creation (validation + success)
  - Comment addition
  - Error handling for missing post
  - Listing posts with comments

Run tests:
```bash
dotnet test
```

---

## 🚀 Run Locally
```bash
dotnet restore
dotnet run --project BlogPlatform.Api/BlogPlatform.Api.csproj
```
Swagger UI available at:
- `http://localhost:5000/`
- `https://localhost:5001/`

---

## 🧩 Project Structure
```
BlogPlatform/
├── BlogPlatform.sln
├── BlogPlatform.Api/
│   ├── Controllers/
│   ├── Services/
│   ├── Data/
│   ├── Middleware/
│   ├── Models/
│   ├── Program.cs
│   └── appsettings.Development.json
├── BlogPlatform.Tests/
│   ├── Services/
│   └── Helpers/
└── .gitignore
```

---

## 🧰 Tech Stack
| Category | Tool |
|-----------|------|
| Framework | .NET 8 (ASP.NET Core Web API) |
| ORM | Entity Framework Core (InMemory) |
| Auth | JWT Bearer |
| Docs | Swagger / Swashbuckle.AspNetCore |
| Tests | xUnit + Microsoft.NET.Test.Sdk |
| IDE | Visual Studio 2022 / VS Code |

---

## 👨‍💻 Author
**Juan Vinicius Panighel**  
Systems Developer 
💻 .NET | C# | EF Core | React | TypeScript | CI/CD  
📍 Brazil  
