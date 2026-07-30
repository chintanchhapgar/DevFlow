using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.Delete;

public sealed class DeleteEpicEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/epics/{epicId:guid}",
            async (
                Guid epicId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new DeleteEpicCommand(epicId),
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Epics")
            .WithName("Epics_Delete")
            .WithSummary("Delete epic")
            .WithDescription("Soft deletes an epic.")
            .Produces<DeleteEpicResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
