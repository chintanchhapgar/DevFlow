using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Worklogs.Delete;

public sealed class DeleteWorklogEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/worklogs/{worklogId:guid}",
            async (
                Guid worklogId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new DeleteWorklogCommand(worklogId),
                        cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Worklogs")
            .WithName("Worklogs_Delete")
            .WithSummary("Delete worklog")
            .WithDescription("Soft deletes a worklog.")
            .Produces<DeleteWorklogResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
