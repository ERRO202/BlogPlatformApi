# BlogPlatform.Api (NET 8, InMemory, Swagger @ /, JWT)

Thin Controllers → Services → Data. Global ErrorHandlingMiddleware. All endpoints return `Result<T>`.
JWT Bearer auth protects **POST** actions; listing/detail are anonymous.

## Run
```bash
dotnet restore
dotnet run --project BlogPlatform.Api/BlogPlatform.Api.csproj
```
Swagger UI (Development): http://localhost:5000/ (or https://localhost:5001/)

## Auth (JWT)
- Get token:
```bash
curl -X POST http://localhost:5000/api/auth/token \
  -H "Content-Type: application/json" \
  -d '{ "username": "juan", "password": "dev" }'
```
Response (`Result<ResponseTokenResponse>`):
```json
{ "success": true, "data": { "token": "<JWT>", "expiresAtUtc": "..." }, "errors": [] }
```
- Use token in Swagger (Authorize → `Bearer <JWT>`) or in cURL:
```bash
curl -H "Authorization: Bearer <JWT>" http://localhost:5000/api/posts
```

## Endpoints
- `GET /api/posts` → `Result<List<ResponseBlogPostListResponse>>` (anonymous)
- `GET /api/posts/{id}` → `Result<ResponseBlogPostDetailResponse>` (anonymous)
- `POST /api/posts` → `Result<ResponseBlogPostDetailResponse>` (requires Bearer)
- `POST /api/posts/{id}/comments` → `Result<ResponseCommentResponse>` (requires Bearer)

## Notes
- InMemory DB; data resets on each run.
- JWT settings in `appsettings.Development.json` (`Jwt:Issuer`, `Audience`, `Key`). **Change the key in production.**

## Switch to SQL Server (optional)
1) Add packages:
```
dotnet add BlogPlatform.Api package Microsoft.EntityFrameworkCore.SqlServer
dotnet add BlogPlatform.Api package Microsoft.EntityFrameworkCore.Tools
```
2) Update `Program.cs` to `UseSqlServer(...)` and add a connection string.
3) Run migrations: `dotnet ef migrations add Initial && dotnet ef database update`.
