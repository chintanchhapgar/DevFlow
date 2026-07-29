using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace DevFlow.BuildingBlocks.Security.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGenWithJwt(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            // ADD THIS
            options.CustomSchemaIds(type =>
                type.FullName!.Replace("+", "."));

            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "DevFlow API",
                    Version = "v1"
                });

            var securityScheme =
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description =
                        "Enter JWT Bearer token only",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                };

            options.AddSecurityDefinition(
                "Bearer",
                securityScheme);

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        Array.Empty<string>()
                    }
                });
        });

        return services;
    }
}
