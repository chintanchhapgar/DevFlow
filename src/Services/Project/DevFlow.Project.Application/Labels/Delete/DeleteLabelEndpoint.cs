using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Labels.Delete;

public sealed class DeleteLabelEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/labels/{labelId:guid}",
            async (
                Guid labelId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new DeleteLabelCommand(labelId),
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Labels")
            .WithName("Labels_Delete")
            .WithSummary("Delete label")
            .WithDescription("Soft deletes a project label.")
            .Produces<DeleteLabelResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
