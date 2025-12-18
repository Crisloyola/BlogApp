using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BlogApp.Domain.Ports.Out;
using BlogApp.Infrastructure.Persistence.Context;
using BlogApp.Infrastructure.Persistence.Repositories;

namespace BlogApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // 🔹 Variables de entorno (NOMBRES CORRECTOS)
        var host = Environment.GetEnvironmentVariable("DB_HOST");
        var port = Environment.GetEnvironmentVariable("DB_PORT");
        var database = Environment.GetEnvironmentVariable("DB_NAME");
        var user = Environment.GetEnvironmentVariable("DB_USER");
        var password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        // 🔹 Validación (muy recomendada)
        if (string.IsNullOrWhiteSpace(host))
            throw new Exception("DB_HOST no está definido en el .env");

        // 🔹 Connection String
        var connectionString =
            $"Server={host};Port={port};Database={database};User={user};Password={password};";

        Console.WriteLine("✔ MySQL ConnectionString cargada correctamente");

        // 🔹 DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 0))
            )
        );

        // 🔹 Repositorios
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
