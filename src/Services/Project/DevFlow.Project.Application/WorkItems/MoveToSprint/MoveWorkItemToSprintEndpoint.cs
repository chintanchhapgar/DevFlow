using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.WorkItems.MoveToSprint;

public sealed class MoveWorkItemToSprintEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/work-items/{workItemId:guid}/sprint",
            async (
                Guid workItemId,
                MoveWorkItemToSprintRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new MoveWorkItemToSprintCommand(
                    workItemId,
                    request.SprintId);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Work Items")
            .WithName("MoveWorkItemToSprint")
            .WithSummary("Move work item to sprint")
            .Produces<MoveWorkItemToSprintResponse>()
            .RequireAuthorization();
    }
}
