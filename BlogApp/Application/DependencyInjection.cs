using Microsoft.Extensions.DependencyInjection;
using BlogApp.Application.Interfaces;
using BlogApp.Application.Services;
using BlogApp.Application.Mappings;
using FluentValidation;
using BlogApp.Application.Validators;

namespace BlogApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddValidatorsFromAssemblyContaining<CreatePostDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateAuthorDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<CreateCommentDtoValidator>();


            return services;
        }
    }
}