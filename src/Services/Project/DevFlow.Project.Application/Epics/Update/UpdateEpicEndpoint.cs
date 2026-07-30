using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.Update;

public sealed class UpdateEpicEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/epics/{epicId:guid}",
            async (
                Guid epicId,
                UpdateEpicRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateEpicCommand(
                    epicId,
                    request.Name,
                    request.Description,
                    request.Color,
                    request.StartDate,
                    request.DueDate);

                var result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Epics")
            .WithName("Epics_Update")
            .WithSummary("Update epic")
            .WithDescription("Updates an existing epic.")
            .Produces<UpdateEpicResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }
}
