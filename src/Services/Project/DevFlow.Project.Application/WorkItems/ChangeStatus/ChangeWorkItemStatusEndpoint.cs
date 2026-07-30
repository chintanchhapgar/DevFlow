using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.WorkItems.ChangeStatus;

public sealed class ChangeWorkItemStatusEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/work-items/{workItemId:guid}/status",
            async (
                Guid workItemId,
                ChangeWorkItemStatusRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new ChangeWorkItemStatusCommand(
                    workItemId,
                    request.Status);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Work Items")
            .WithName("ChangeWorkItemStatus")
            .WithSummary("Change work item status")
            .WithDescription("Updates the workflow status of a work item.")
            .Produces<ChangeWorkItemStatusResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
