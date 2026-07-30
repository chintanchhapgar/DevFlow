using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Worklogs.Update;

public sealed class UpdateWorklogEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/worklogs/{worklogId:guid}",
            async (
                Guid worklogId,
                UpdateWorklogRequest request,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateWorklogCommand(
                    worklogId,
                    request.Description,
                    request.StartedAtUtc,
                    request.EndedAtUtc);

                var result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Worklogs")
            .WithName("Worklogs_Update")
            .WithSummary("Update worklog")
            .Produces<UpdateWorklogResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
