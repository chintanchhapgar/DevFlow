using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Middleware;
using DevFlow.BuildingBlocks.Infrastructure;
using DevFlow.BuildingBlocks.Messaging;
using DevFlow.BuildingBlocks.Messaging.Configuration;
using DevFlow.BuildingBlocks.Security.Extensions;
using DevFlow.Notification.Application;
using DevFlow.Notification.Application.Notifications.GetMyNotifications;
using DevFlow.Notification.Infrastructure;
using DevFlow.Notification.Infrastructure.Email.Configuration;
using DevFlow.Notification.Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructureServices();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddMessaging(
    builder.Configuration,
    consumerAssemblies:
    [
        typeof(Program).Assembly
    ]);

builder.Services
    .AddOptions<EmailSettings>()
    .BindConfiguration(EmailSettings.SectionName)
    .Validate(
        settings => Uri.TryCreate(
            settings.FrontendBaseUrl,
            UriKind.Absolute,
            out _),
        "Email:FrontendBaseUrl must be a valid absolute URL.")
    .ValidateOnStart();
const string DevFlowCorsPolicy = "DevFlowCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        DevFlowCorsPolicy,
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5174",
                    "https://localhost:5174",
                    "http://localhost:5173",
                    "https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(DevFlowCorsPolicy);

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
