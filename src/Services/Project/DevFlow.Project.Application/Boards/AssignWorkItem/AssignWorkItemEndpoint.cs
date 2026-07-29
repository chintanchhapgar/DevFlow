using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Boards.AssignWorkItem;

public sealed class AssignWorkItemEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/workitems/{workItemId:guid}/assign",
            async (
                Guid workItemId,
                AssignWorkItemRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new AssignWorkItemCommand(
                    workItemId,
                    request.AssigneeId);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Boards")
            .WithName("AssignWorkItemForBoards")
            .WithSummary("Assign work item")
            .WithDescription("Assigns a work item to a user.")
            .Produces<AssignWorkItemResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
