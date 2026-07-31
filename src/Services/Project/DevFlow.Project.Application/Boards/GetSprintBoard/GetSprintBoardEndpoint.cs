using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Boards.GetSprintBoard;

public sealed class GetSprintBoardEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/sprints/{sprintId:guid}/board",
            async (
                Guid sprintId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetSprintBoardQuery(sprintId),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Boards")
            .WithName("GetSprintBoard")
            .WithSummary("Get sprint board")
            .WithDescription("Returns the specified sprint board grouped by status.")
            .Produces<GetSprintBoardResponse>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
