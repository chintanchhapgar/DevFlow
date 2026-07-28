using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Sprints.Start;

public sealed class StartSprintEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/sprints/{sprintId:guid}/start",
            async (
                Guid sprintId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new StartSprintCommand(sprintId),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Sprints")
            .WithName("StartSprint")
            .WithSummary("Start sprint")
            .WithDescription("Starts a planned sprint.")
            .Produces<StartSprintResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
