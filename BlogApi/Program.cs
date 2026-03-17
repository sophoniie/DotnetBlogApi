using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using BlogApi.Data;
using BlogApi.Repositories.Users;
using BlogApi.Repositories.Articles;
using BlogApi.Repositories.Categories;
using BlogApi.Repositories.Tags;
using BlogApi.Services.Users;
using BlogApi.Services;
using BlogApi.Services.Token;
using BlogApi.Services.Auth;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var postgresSection = builder.Configuration.GetSection("Postgres");

var host = builder.Configuration["Postgres:Host"] ?? throw new InvalidOperationException("Postgres:Host is not configured");
var port = builder.Configuration["Postgres:Port"] ?? throw new InvalidOperationException("Postgres:Port is not configured");
var database = builder.Configuration["Postgres:Database"] ?? throw new InvalidOperationException("Postgres:Database is not configured");
var user = builder.Configuration["Postgres:Username"] ?? throw new InvalidOperationException("Postgres:Username is not configured");
var password = builder.Configuration["Postgres:Password"] ?? throw new InvalidOperationException("Postgres:Password is not configured");

var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";

var jwtSecretKey = builder.Configuration["Jwt:Key"] 
    ?? throw new InvalidOperationException("Jwt:Key is not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
    ?? throw new InvalidOperationException("Jwt:Issuer is not configured");
var jwtAudience = builder.Configuration["Jwt:Audience"] 
    ?? throw new InvalidOperationException("Jwt:Audience is not configured");

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddCors();
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            
            ValidateLifetime = true,
            
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
