using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Sprints.Create;

public sealed class CreateSprintEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/projects/{projectId:guid}/sprints",
            async (
                Guid projectId,
                CreateSprintRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateSprintCommand(
                    projectId,
                    request.Name,
                    request.Goal,
                    request.StartDate,
                    request.EndDate);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Sprints")
            .WithName("CreateSprint")
            .WithSummary("Create sprint")
            .WithDescription("Creates a new sprint for a project.")
            .Produces<CreateSprintResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
