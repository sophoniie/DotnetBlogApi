# AGENTS.md - Guidelines for Agentic Coding in This Repository

## 1. Build/Lint/Test Commands

| Command | Description |
|---------|-------------|
| `cd BlogApi && dotnet build` | Build the project |
| `cd BlogApi && dotnet run` | Run the API |
| `dotnet test` | Run all tests |

**Single test command** (once tests exist):
```bash
dotnet test --filter "FullyQualifiedName~TestClassName.TestMethodName"
```

---

## 2. Code Style Guidelines

### Documentation & Research
- **ALWAYS** look up documentation from official sources when working with unfamiliar libraries/frameworks
- **Preferred sources**:
  - **Context7 MCP** - Use `context7_resolve-library-id` and `context7_query-docs` for .NET libraries (Entity Framework Core, FluentValidation, etc.)
  - **Web Search** - Use `websearch` for recent best practices and tutorials
  - **Official Docs**: dotnet.microsoft.com, docs.microsoft.com, GitHub repositories
- Before implementing anything new, search for official documentation and best practices

### Project Configuration
- **.NET 10.0** with `Nullable=enable` and `ImplicitUsings=enable`
- **Target OS**: Linux (for Docker)

### Naming Conventions
- **Classes/Types**: PascalCase (`User`, `ArticleResponse`)
- **Methods/Properties**: PascalCase (`UpdateInfo`, `AuthorId`)
- **Enums**: PascalCase with descriptive names (`RoleEnum`, `ArticleStatusEnum`)
- **DTOs**: 
  - Request: `{Action}{Entity}Request` (e.g., `RegisterUserRequest`, `CreateArticleRequest`)
  - Response: `{Entity}Response` (e.g., `UserResponse`, `ArticleResponse`)
  - Shared: `{Description}Response` (e.g., `AuthorSummaryResponse`)

### File Organization
```
BlogApi/
├── Data/
│   ├── BlogDbContext.cs
│   └── Configurations/          # EF Core IEntityTypeConfiguration
├── DTOs/
│   ├── Users/                   # User-related DTOs
│   ├── Articles/                # Article-related DTOs
│   └── Shared/                  # Shared DTOs
├── Models/
│   ├── BaseEntity.cs            # Abstract base with Id, CreatedAt, UpdatedAt
│   ├── User.cs
│   ├── Article.cs
│   └── Enums/                   # Enums folder
├── Validators/
│   ├── Users/                   # FluentValidation validators
│   └── Articles/
└── Program.cs
```

### Type Conventions
- **Strings**: Use `null!` for required strings (not `string.Empty`)
  ```csharp
  public string Name { get; set; } = null!;
  ```
- **Nullable**: Use `?` for optional types
  ```csharp
  public DateTime? DeletedAt { get; set; }
  public string? Thumbnail { get; set; }
  ```
- **Collections**: Initialize with `= null!` (EF Core will inject)
  ```csharp
  public ICollection<Article> Articles { get; set; } = null!;
  ```

### Entity Modeling
- **BaseEntity**: All entities inherit from `BaseEntity` (Id, CreatedAt, UpdatedAt)
- **Relationships**: Configure in separate files under `Data/Configurations/`
- **Query Filters**: Global filters in `BlogDbContext.OnModelCreating`
- **Soft Delete**: Implemented via `DeletedAt` property + global query filter
- **Assembly Discovery**: Use `modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())`
- **Cascade Delete**: Use `DeleteBehavior.Restrict` when relationship requires it (e.g., Article requires Author)

### Validation (FluentValidation)
- All validators in `Validators/` folder
- Inherit from `AbstractValidator<T>`
- Use chainable methods: `.NotEmpty()`, `.WithMessage()`, `.MaximumLength()`, etc.
- Password validation: Require minimum 8 characters with complexity (uppercase, lowercase, number, special char)

### Configuration Priority
1. **Environment variables** (highest priority)
2. **appsettings.json** defaults

Example pattern:
```csharp
var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") 
    ?? postgresSection["Host"];
```

### Error Handling
- Use exceptions with descriptive messages
- TODO comments for incomplete implementations (e.g., password hashing not yet implemented)

### Model Methods
- **Entity methods**: Include methods for common operations (e.g., `SoftDelete()`, `Restore()`, `Publish()`, `SetThumbnail()`)
- **Constructors**: Provide constructors that initialize common properties (CreatedAt, UpdatedAt, default values)
- **Update methods**: Separate methods for updating different aspects (e.g., `UpdateInfo()` vs `UpdatePassword()`)

---

## 3. Packages Used
- `Microsoft.EntityFrameworkCore` 10.0.5
- `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.1
- `FluentValidation.DependencyInjectionExtensions` 12.1.1

---

## 4. Docker & Database
- **PostgreSQL 18-alpine** configured in `docker-compose.yml`
- **Connection**: Configure via environment variables or `appsettings.json`
- **Migrations**: Use `dotnet ef` commands (when needed)
- **Volume**: Use explicit volume names for easier tracking in logs

---

## 5. API Design Principles
- Use **DTOs** for all API requests and responses (never expose domain models directly)
- **Request DTOs**: For input validation
- **Response DTOs**: For output shaping (exclude sensitive data like passwords)
- Use **FluentValidation** for all input validation
- Handle status changes via entity methods (not separate DTOs)
