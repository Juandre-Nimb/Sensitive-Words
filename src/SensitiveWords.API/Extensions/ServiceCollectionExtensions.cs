using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using SensitiveWordsClean.Application.Interfaces;
using SensitiveWordsClean.Application.Mappings;
using SensitiveWordsClean.Application.Services;
using SensitiveWordsClean.Application.Validators;
using SensitiveWordsClean.Domain.Interfaces;
using SensitiveWordsClean.Infrastructure.Repositories;
using SensitiveWordsClean.Infrastructure.Services;

namespace SensitiveWordsClean.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ISensitiveWordService, SensitiveWordService>();
        services.AddScoped<ITextSanitizationService, TextSanitizationService>();

        services.AddAutoMapper(typeof(SensitiveWordMappingProfile));
        services.AddMemoryCache();
        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddValidatorsFromAssemblyContaining<CreateSensitiveWordDtoValidator>();

        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISensitiveWordRepository, SensitiveWordRepository>();

        return services;
    }

    public static IServiceCollection AddApiDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Sensitive Words Clean API",
                Version = "v1",
                Description = "A comprehensive API for managing sensitive words and text sanitization",                
            });

            var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            c.EnableAnnotations();
        });

        return services;
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowWebApp", builder =>
            {
                builder.WithOrigins("https://localhost:5002", "http://localhost:5002")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials();
            });

            options.AddPolicy("Development", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });

        return services;
    }
}
