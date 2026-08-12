using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Middleware;
using DevFlow.BuildingBlocks.Infrastructure;
using DevFlow.BuildingBlocks.Messaging;
using DevFlow.BuildingBlocks.Security.Extensions;
using DevFlow.Identity.Application;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Infrastructure;
using DevFlow.Identity.Infrastructure.Requests;
using DevFlow.Identity.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructureServices();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddScoped<ICurrentRequestInfo, CurrentRequestInfo>();

builder.Services.AddMessaging(
    builder.Configuration,
    consumerAssemblies:
    [
        typeof(Program).Assembly
    ]);

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

await app.SeedDemoDataAsync();

app.Run();
