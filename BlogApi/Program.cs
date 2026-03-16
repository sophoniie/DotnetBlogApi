using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var postgresSection = builder.Configuration.GetSection("Postgres");

var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? postgresSection["Host"];
var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? postgresSection["Port"];
var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? postgresSection["Database"];
var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? postgresSection["Username"];
var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? postgresSection["Password"];

var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";

builder.Services.AddDbContext<DbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
