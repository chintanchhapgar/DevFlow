using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.RemoveWorkItem;

public sealed class RemoveWorkItemEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/epics/{epicId:guid}/work-items/{workItemId:guid}",
            async (
                Guid epicId,
                Guid workItemId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new RemoveWorkItemCommand(
                        epicId,
                        workItemId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Epics")
            .WithName("Epics_RemoveWorkItem")
            .WithSummary("Remove work item from epic")
            .Produces<RemoveWorkItemResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
