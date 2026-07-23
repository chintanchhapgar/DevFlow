using DevFlow.Identity.API.Authentication;
using DevFlow.Identity.API.Endpoints;
using DevFlow.Identity.Application;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Infrastructure;


using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevFlow Identity API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT Bearer token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.MapGet("/", () => "DevFlow Identity Service");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapRegisterEndpoint();

app.MapLoginEndpoint();

app.MapRefreshTokenEndpoint();

app.MapLogoutEndpoint();

app.MapProfileEndpoint();

app.MapForgotPasswordEndpoint();

app.MapResetPasswordEndpoint();

app.MapChangePasswordEndpoint();

app.Run();
