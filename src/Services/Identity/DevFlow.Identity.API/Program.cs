using DevFlow.BuildingBlocks.Api.Middleware;
using DevFlow.BuildingBlocks.Security.Extensions;
using DevFlow.Identity.Api.Extensions;
using DevFlow.Identity.Application;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Infrastructure;
using DevFlow.Identity.Infrastructure.Requests;
using DevFlow.Identity.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddSecurity(builder.Configuration);

builder.Services.AddScoped<ICurrentRequestInfo, CurrentRequestInfo>();

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

await app.SeedDemoDataAsync();

app.Run();
