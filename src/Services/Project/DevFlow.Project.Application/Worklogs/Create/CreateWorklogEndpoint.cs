using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Worklogs.Create;

public sealed class CreateWorklogEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/worklogs",
            async (
                CreateWorklogRequest request,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateWorklogCommand(
                    request.WorkItemId,
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
            .WithName("Worklogs_Create")
            .WithSummary("Create worklog")
            .WithDescription("Creates a new worklog entry.")
            .Produces<CreateWorklogResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
