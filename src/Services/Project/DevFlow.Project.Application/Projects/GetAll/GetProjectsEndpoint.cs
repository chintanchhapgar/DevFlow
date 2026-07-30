using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.SharedKernel.Pagination;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                [AsParameters] PaginationRequest pagination,
                string? search,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
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
            .Produces<PagedList<ProjectListItemResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.ProjectViewer);
    }
}
