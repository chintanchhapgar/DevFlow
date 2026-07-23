using DevFlow.Identity.Application;
using DevFlow.Identity.Infrastructure;
using DevFlow.Identity.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => "DevFlow Identity Service");
app.MapRegisterEndpoint();
app.Run();
