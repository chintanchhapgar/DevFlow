using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Reports.Velocity;

public sealed class GetVelocityEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/reports/velocity",
            async (
                Guid projectId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetVelocityQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Reports")
            .WithName("GetVelocity")
            .WithSummary("Project velocity")
            .WithDescription("Returns sprint velocity for the project.")
            .Produces<GetVelocityResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.Reports);
    }
}
