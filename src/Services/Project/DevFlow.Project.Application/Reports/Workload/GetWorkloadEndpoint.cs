using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Reports.Workload;

public sealed class GetWorkloadEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/reports/workload",
            async (
                Guid projectId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetWorkloadQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Reports")
            .WithName("GetWorkload")
            .WithSummary("Project workload")
            .WithDescription("Returns workload grouped by assignee.")
            .Produces<GetWorkloadResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.Reports);
    }
}
