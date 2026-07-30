using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Labels.RemoveFromWorkItem;

public sealed class RemoveLabelFromWorkItemEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/work-items/{workItemId:guid}/labels/{labelId:guid}",
            async (
                Guid workItemId,
                Guid labelId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command =
                    new RemoveLabelFromWorkItemCommand(
                        workItemId,
                        labelId);

                var result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Labels")
            .WithName("Labels_RemoveFromWorkItem")
            .WithSummary("Remove label from work item")
            .WithDescription("Removes a label from a work item.")
            .Produces<RemoveLabelFromWorkItemResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
