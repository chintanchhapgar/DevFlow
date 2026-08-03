using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Reports.Burndown;

public sealed class GetBurndownEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/sprints/{sprintId:guid}/reports/burndown",
            async (
                Guid sprintId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetBurndownQuery(sprintId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Reports")
            .WithName("GetBurndown")
            .WithSummary("Sprint burndown")
            .Produces<GetBurndownResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.ProjectViewer);
    }
}
