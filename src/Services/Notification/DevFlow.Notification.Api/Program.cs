using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Middleware;
using DevFlow.BuildingBlocks.Messaging;
using DevFlow.BuildingBlocks.Messaging.Configuration;
using DevFlow.BuildingBlocks.Security.Extensions;
using DevFlow.Notification.Application;
using DevFlow.Notification.Application.Notifications.GetMyNotifications;
using DevFlow.Notification.Infrastructure;
using DevFlow.Notification.Infrastructure.Persistence;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddMessaging(
    builder.Configuration,
    consumerAssemblies:
    [
        typeof(Program).Assembly
    ]);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();
