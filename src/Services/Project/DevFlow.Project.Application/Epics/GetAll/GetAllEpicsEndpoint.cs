using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.GetAll;

public sealed class GetAllEpicsEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/epics",
            async (
                Guid projectId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new GetAllEpicsQuery(projectId),
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Epics")
            .WithName("Epics_GetAll")
            .WithSummary("Get project epics")
            .WithDescription("Returns all epics for a project.")
            .Produces<IReadOnlyList<GetAllEpicsResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}
