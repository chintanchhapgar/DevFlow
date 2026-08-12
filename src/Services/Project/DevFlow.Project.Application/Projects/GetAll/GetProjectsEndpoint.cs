using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.SharedKernel.Pagination;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.GetAll;

public sealed class GetProjectsEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects",
            async (
                int? page,
                int? pageSize,
                string? sortBy,
                string? sortDirection,
                string? search,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var pagination = new PaginationRequest
                {
                    Page = page ?? 1,
                    PageSize = pageSize ?? 20,
                    SortBy = sortBy,
                    SortDirection = sortDirection ?? "asc"
                };

                var query = new GetProjectsQuery(
                    pagination,
                    search);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
        .WithTags("Projects")
        .WithName("Projects_GetAll")
        .WithSummary("Get paged projects")
        .WithDescription("Returns a paginated list of projects.")
        .Produces<PagedList<ProjectListItemResponse>>(
            StatusCodes.Status200OK)
        .RequireAuthorization(
            PolicyNames.ProjectViewer);
    }
}
