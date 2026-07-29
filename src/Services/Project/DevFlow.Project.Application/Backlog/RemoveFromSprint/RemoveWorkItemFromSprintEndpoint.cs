using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Backlog.RemoveFromSprint;

public sealed class RemoveWorkItemFromSprintEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/workitems/{workItemId:guid}/sprint",
            async (
                Guid workItemId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new RemoveWorkItemFromSprintCommand(
                        workItemId),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Backlog")
            .WithName("RemoveWorkItemFromSprint")
            .WithSummary("Remove work item from sprint")
            .WithDescription("Moves a work item back to the product backlog.")
            .Produces<RemoveWorkItemFromSprintResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
