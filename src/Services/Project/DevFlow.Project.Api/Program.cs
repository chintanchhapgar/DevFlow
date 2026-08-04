using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Messaging;
using DevFlow.BuildingBlocks.Security.Extensions;
using DevFlow.Project.Application;
using DevFlow.Project.Infrastructure;
using DevFlow.Project.Infrastructure.Persistence;
using DevFlow.Project.Infrastructure.Seed;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------------------------------
// Logging
// ------------------------------------------------------------

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// ------------------------------------------------------------
// Services
// ------------------------------------------------------------

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddMessaging(
    builder.Configuration,
    consumerAssemblies:
    [
        typeof(Program).Assembly
    ]);

// Shared Security
builder.Services.AddSecurity(builder.Configuration);

var app = builder.Build();

// ------------------------------------------------------------
// Database
// ------------------------------------------------------------


//await app.SeedDemoDataAsync();

// ------------------------------------------------------------
// Middleware
// ------------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DevFlow Project API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

// ------------------------------------------------------------
// Endpoints
// ------------------------------------------------------------

app.MapEndpoints();

app.Run();

public partial class Program;
