using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.WorkItems.ChangePriority;

public sealed class ChangeWorkItemPriorityEndpoint
    : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/work-items/{workItemId:guid}/priority",
            async (
                Guid workItemId,
                ChangeWorkItemPriorityRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new ChangeWorkItemPriorityCommand(
                    workItemId,
                    request.Priority);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Work Items")
            .WithName("ChangeWorkItemPriority")
            .WithSummary("Change work item priority")
            .WithDescription("Changes the priority of a work item.")
            .Produces<ChangeWorkItemPriorityResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
