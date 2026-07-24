using System.Text;
using System.Text.Json;
using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.Identity.Infrastructure.Authentication;
using DevFlow.SharedKernel.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace DevFlow.Identity.API.Authentication;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var settings = configuration
            .GetSection(JwtSettings.SectionName)
            .Get<JwtSettings>()
            ?? throw new InvalidOperationException(
                "JWT settings are missing.");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer = settings.Issuer,
                        ValidAudience = settings.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(settings.SecretKey)),

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

        services.AddAuthorization();

        return services;
    }
}
