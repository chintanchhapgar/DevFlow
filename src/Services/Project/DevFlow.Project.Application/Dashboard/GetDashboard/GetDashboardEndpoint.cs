using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Dashboard.GetDashboard;

public sealed class GetDashboardEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/dashboard",
            async (
                Guid projectId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetDashboardQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Dashboard")
            .WithName("GetDashboard")
            .WithSummary("Get project dashboard")
            .WithDescription("Returns project dashboard information.")
            .Produces<GetDashboardResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.ProjectViewer);
    }
}
