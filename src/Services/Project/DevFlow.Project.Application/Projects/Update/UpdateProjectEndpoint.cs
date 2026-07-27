using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Update;

public sealed class UpdateProjectEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/projects/{projectId:guid}",
            async (
                Guid projectId,
                UpdateProjectRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateProjectCommand(
                    projectId,
                    request.Name,
                    request.Description,
                    request.Visibility);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithName("UpdateProject")
            .WithSummary("Update project")
            .Produces<UpdateProjectResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
