using System.Text;
using System.Text.Json;
using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DevFlow.BuildingBlocks.Security.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptions = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>()
            ?? throw new InvalidOperationException(
                "JWT configuration is missing.");

        services.Configure<JwtOptions>(
            configuration.GetSection(
                JwtOptions.SectionName));

        services.AddHttpContextAccessor();

        services.AddScoped<ICurrentUser, CurrentUser>();

        services
            .AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;

                options.SaveToken = true;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    jwtOptions.SecretKey)),

                        ClockSkew = TimeSpan.Zero
                    };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode =
                            StatusCodes.Status401Unauthorized;

                        context.Response.ContentType =
                            "application/json";

                        var response = new ApiResponse<object?>
                        {
                            Success = false,
                            Message = "You do not have permission to perform this action.",
                            Data = null,
                            Error = new ApiError
                            {
                                Code = "Authentication.Unauthorized",
                                Type = ErrorType.Unauthorized
                            },
                            TraceId = context.HttpContext.TraceIdentifier,
                            Timestamp = DateTime.UtcNow
                        };

                        await context.Response.WriteAsync(
                            JsonSerializer.Serialize(response));
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode =
                            StatusCodes.Status403Forbidden;

                        context.Response.ContentType =
                            "application/json";

                        var response = new ApiResponse<object?>
                        {
                            Success = false,
                            Message = "Forbidden.",
                            Data = null,
                            Error = new ApiError
                            {
                                Code = "Authorization.Forbidden",
                                Type = ErrorType.Forbidden
                            },
                            TraceId = context.HttpContext.TraceIdentifier,
                            Timestamp = DateTime.UtcNow
                        };

                        await context.Response.WriteAsync(
                            JsonSerializer.Serialize(response));
                    }
                };
            });

        return services;
    }
}
