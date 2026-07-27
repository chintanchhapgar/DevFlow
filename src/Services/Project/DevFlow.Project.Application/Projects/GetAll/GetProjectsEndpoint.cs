using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
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
                int page,
                int pageSize,
                string? search,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var query = new GetProjectsQuery(
                    page == 0 ? 1 : page,
                    pageSize == 0 ? 20 : pageSize,
                    search);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithName("GetProjects")
            .WithSummary("Get paged projects")
            .Produces<GetProjectsResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}
