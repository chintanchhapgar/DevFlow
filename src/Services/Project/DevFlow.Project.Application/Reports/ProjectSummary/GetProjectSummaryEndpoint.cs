using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Reports.ProjectSummary;

public sealed class GetProjectSummaryEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/reports/summary",
            async (
                Guid projectId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetProjectSummaryQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Reports")
            .WithName("GetProjectSummary")
            .WithSummary("Get project summary report")
            .WithDescription("Returns project summary metrics.")
            .Produces<GetProjectSummaryResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.Reports);
    }
}
