using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Backlog.GetBacklog;

public sealed class GetBacklogEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/backlog",
            async (
                Guid projectId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetBacklogQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Backlog")
            .WithName("GetBacklog")
            .WithSummary("Get backlog")
            .WithDescription("Returns all unscheduled work items for a project.")
            .Produces<GetBacklogResponse>(StatusCodes.Status200OK)
           .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
