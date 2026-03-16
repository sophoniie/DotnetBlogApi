using Microsoft.EntityFrameworkCore;
using BlogApi.Data;
using BlogApi.Repositories.Users;
using BlogApi.Repositories.Articles;
using BlogApi.Repositories.Categories;
using BlogApi.Repositories.Tags;

var builder = WebApplication.CreateBuilder(args);

var postgresSection = builder.Configuration.GetSection("Postgres");

var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? postgresSection["Host"];
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? postgresSection["Port"];
var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? postgresSection["Database"];
var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? postgresSection["Username"];
var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? postgresSection["Password"];

var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
