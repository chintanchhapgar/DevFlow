using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Worklogs.GetAll;

public sealed class GetAllWorklogsEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/work-items/{workItemId:guid}/worklogs",
            async (
                Guid workItemId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new GetAllWorklogsQuery(workItemId),
                        cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Worklogs")
            .WithName("Worklogs_GetAll")
            .WithSummary("Get worklogs")
            .WithDescription("Returns all worklogs for a work item.")
            .Produces<IReadOnlyList<GetAllWorklogsResponse>>(StatusCodes.Status200OK)
            .RequireAuthorization(PolicyNames.ProjectViewer);
    }
}
