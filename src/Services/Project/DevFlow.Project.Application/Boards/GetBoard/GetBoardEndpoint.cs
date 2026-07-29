using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Boards.GetBoard;

public sealed class GetBoardEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/projects/{projectId:guid}/board",
            async (
                Guid projectId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetBoardQuery(projectId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Boards")
            .WithName("GetBoard")
            .WithSummary("Get sprint board")
            .WithDescription("Returns the active sprint board grouped by status.")
            .Produces<GetBoardResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}
