using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.AssignWorkItem;

public sealed class AssignWorkItemEndpoint
    : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/epics/{epicId:guid}/work-items",
            async (
                Guid epicId,
                AssignWorkItemRequest request,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new AssignWorkItemCommand(
                        epicId,
                        request.WorkItemId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Epics")
            .WithName("Epics_AssignWorkItem")
            .WithSummary("Assign work item")
            .RequireAuthorization();
    }
}
