using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Labels.AssignToWorkItem;

public sealed class AssignLabelToWorkItemEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/work-items/{workItemId:guid}/labels",
            async (
                Guid workItemId,
                AssignLabelToWorkItemRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new AssignLabelToWorkItemCommand(
                    workItemId,
                    request.LabelId);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Labels")
            .WithName("Labels_AssignToWorkItem")
            .WithSummary("Assign label to work item")
            .WithDescription("Assigns an existing label to a work item.")
            .Produces<AssignLabelToWorkItemResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
