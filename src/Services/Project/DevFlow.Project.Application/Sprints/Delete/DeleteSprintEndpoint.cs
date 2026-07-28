using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Sprints.Delete;

public sealed class DeleteSprintEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/sprints/{sprintId:guid}",
            async (
                Guid sprintId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new DeleteSprintCommand(sprintId),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Sprints")
            .WithName("DeleteSprint")
            .WithSummary("Delete sprint")
            .WithDescription("Soft deletes a sprint.")
            .Produces<DeleteSprintResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
