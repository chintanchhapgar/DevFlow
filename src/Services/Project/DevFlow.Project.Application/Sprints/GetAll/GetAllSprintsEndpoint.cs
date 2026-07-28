using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Sprints.GetAll;

public sealed class GetAllSprintsEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/sprints",
            async (
                Guid projectId,
                int page,
                int pageSize,
                string? search,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllSprintsQuery(
                    projectId,
                    page <= 0 ? 1 : page,
                    pageSize <= 0 ? 20 : pageSize,
                    search);

                var result = await sender.Send(
                    query,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Sprints")
            .WithName("GetAllSprints")
            .WithSummary("Get all sprints")
            .WithDescription("Returns paginated sprints for a project.")
            .Produces<GetAllSprintsResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}
