using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Sprints.Update;

public sealed class UpdateSprintEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/sprints/{sprintId:guid}",
            async (
                Guid sprintId,
                UpdateSprintRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateSprintCommand(
                    sprintId,
                    request.Name,
                    request.Goal,
                    request.StartDate,
                    request.EndDate);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Sprints")
            .WithName("UpdateSprint")
            .WithSummary("Update sprint")
            .WithDescription("Updates a planned sprint.")
            .Produces<UpdateSprintResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
