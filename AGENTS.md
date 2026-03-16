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
│   ├── Categories/              # Category-related DTOs
│   ├── Tags/                   # Tag-related DTOs
│   └── Shared/                 # Shared DTOs
├── Models/
│   ├── BaseEntity.cs            # Abstract base with Id, CreatedAt, UpdatedAt
│   ├── User.cs
│   ├── Article.cs
│   ├── Category.cs
│   ├── Tag.cs
│   ├── ArticleTag.cs           # Join table for many-to-many
│   └── Enums/                  # Enums folder
├── Repositories/
│   ├── Users/                  # IUserRepository, UserRepository
│   ├── Articles/               # IArticleRepository, ArticleRepository
│   ├── Categories/             # ICategoryRepository, CategoryRepository
│   └── Tags/                   # ITagRepository, TagRepository
├── Validators/
│   ├── Users/                  # FluentValidation validators
│   ├── Articles/
│   ├── Categories/
│   └── Tags/
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
- **Soft Delete**: All entities use soft delete pattern:
  - Add `DeletedAt` property (`DateTime?`)
  - Add `Remove()` method: sets `DeletedAt = DateTime.UtcNow`, updates `UpdatedAt`
  - Add `Restore()` method: sets `DeletedAt = null`, updates `UpdatedAt`
- **Relationships**: Configure in separate files under `Data/Configurations/`
- **Query Filters**: Global filters in `BlogDbContext.OnModelCreating` to hide soft-deleted records
- **Assembly Discovery**: Use `modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly())`
- **Cascade Delete**: Use `DeleteBehavior.Restrict` when relationship requires it (e.g., Article requires Author)

### Repository Pattern (BEST PRACTICES)
- **Primary Constructor**: Use C# 12 primary constructor for repositories
  ```csharp
  public class ArticleRepository(BlogDbContext context) : IArticleRepository
  ```
- **NO Boolean Flags**: Never use boolean parameters for optional includes
  - **BAD**: `GetByIdAsync(int id, bool includeAuthor = false, bool includeCategory = false)`
  - **GOOD**: Create explicit methods like `GetByIdAsync(id)` and `GetByIdWithDetailsAsync(id)`
- **Explicit Methods**: Create specific query methods for each use case
  ```csharp
  // Instead of boolean flags, create explicit methods:
  Task<Article?> GetByIdAsync(int id);
  Task<Article?> GetByIdWithDetailsAsync(int id);  // includes Author, Category, Tags
  Task<IEnumerable<Article>> GetAllAsync(int pageNumber, int pageSize);
  Task<IEnumerable<Article>> GetAllWithDetailsAsync(int pageNumber, int pageSize);
  Task<IEnumerable<Article>> GetPublishedAsync(int pageNumber, int pageSize);
  Task<IEnumerable<Article>> GetPublishedBetweenAsync(DateTime from, DateTime to, int pageNumber, int pageSize);
  ```
- **Service Layer**: Handles relationship orchestration (not repository)
- **Repository Responsibility**: Only persist and retrieve aggregates

### Validation (FluentValidation)
- All validators in `Validators/` folder
- Inherit from `AbstractValidator<T>`
- Use chainable methods: `.NotEmpty()`, `.WithMessage()`, `.MaximumLength()`, etc.
- Password validation: Require minimum 8 characters with complexity (uppercase, lowercase, number, special char)
- **Async Unique Checks**: Inject repository for uniqueness validation
  ```csharp
  public class CreateCategoryValidator : AbstractValidator<CreateCategoryRequest>
  {
      public CreateCategoryValidator(ICategoryRepository categoryRepository)
      {
          RuleFor(x => x.Slug)
              .MustAsync(async (slug, cancellation) => 
                  !await categoryRepository.SlugExistsAsync(slug))
              .WithMessage("Slug already exists");
      }
  }
  ```
- **Slug Validation**: Use regex pattern for URL-friendly slugs
  ```csharp
  .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
  ```
- **FluentValidation Registration**: Use automatic registration in Program.cs
  ```csharp
  builder.Services.AddFluentValidationAutoValidation();
  builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
  ```

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
- **Entity methods**: Include methods for common operations
  - Soft delete: `SoftDelete()`, `Restore()`, `Remove()`
  - Status changes: `Publish()`, `Unpublish()`, `SetToDraft()`
  - Updates: `UpdateInfo()`, `UpdatePassword()`, `SetThumbnail()`, `SetMeta()`, etc.
- **Constructors**: Provide constructors that initialize common properties (CreatedAt, UpdatedAt, default values)
- **Update methods**: Separate methods for updating different aspects (e.g., `UpdateInfo()` vs `UpdatePassword()`)

---

## 3. Packages Used
- `Microsoft.EntityFrameworkCore` 10.0.5
- `Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.1
- `FluentValidation.AspNetCore` 11.3.0
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
