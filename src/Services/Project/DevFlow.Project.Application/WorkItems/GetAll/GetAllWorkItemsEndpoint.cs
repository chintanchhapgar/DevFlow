using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.Project.Domain.WorkItems.Enums;
using DevFlow.SharedKernel.Pagination;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.WorkItems.GetAll;

public sealed class GetAllWorkItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/work-items",
            async (
                Guid projectId,
                [AsParameters] PaginationRequest pagination,
                string? search,
                WorkItemStatus? status,
                WorkItemType? type,
                WorkItemPriority? priority,
                Guid? assigneeId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetAllWorkItemsQuery(
                        projectId,
                        pagination,
                        search,
                        status,
                        type,
                        priority,
                        assigneeId),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Work Items")
            .WithName("GetAllWorkItems")
            .WithSummary("Get all work items")
            .Produces<PagedList<WorkItemListItemResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
