using DotNetEnv;
using BlogApp.API;
using BlogApp.API.Middleware;
using BlogApp.Application;
using BlogApp.Infrastructure;

var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (!File.Exists(envPath))
{
    envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
}

if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine($"Loaded .env from: {envPath}");
}

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS

var allowOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
 ?? new[] { "http://localhost:5173" }; // Valor por defecto si no se encuentra en la configuraci�n

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(allowOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructure();
builder.Services.AddApplication();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidatorFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();


// Usar CORS antes de otros middlewares
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();