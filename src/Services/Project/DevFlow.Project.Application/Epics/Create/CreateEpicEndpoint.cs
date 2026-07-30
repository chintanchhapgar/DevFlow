using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Epics.Create;

public sealed class CreateEpicEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/epics",
            async (
                CreateEpicRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateEpicCommand(
                    request.ProjectId,
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
            .WithName("Epics_Create")
            .WithSummary("Create epic")
            .WithDescription("Creates a new epic.")
            .Produces<CreateEpicResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }
}
