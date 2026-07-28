using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.WorkItems.Create;

public sealed class CreateWorkItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/projects/{projectId:guid}/work-items",
            async (
                Guid projectId,
                CreateWorkItemRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateWorkItemCommand(
                    projectId,
                    request.Title,
                    request.Description,
                    request.Type,
                    request.Priority,
                    request.AssigneeId,
                    request.DueDate,
                    request.EstimateHours);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Work Items")
            .WithName("CreateWorkItem")
            .WithSummary("Create work item")
            .Produces<CreateWorkItemResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}
