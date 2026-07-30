using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.GetById;

public sealed class GetEpicByIdEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/epics/{epicId:guid}",
            async (
                Guid epicId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new GetEpicByIdQuery(epicId),
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Epics")
            .WithName("Epics_GetById")
            .WithSummary("Get epic by id")
            .WithDescription("Returns a single epic.")
            .Produces<GetEpicByIdResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
