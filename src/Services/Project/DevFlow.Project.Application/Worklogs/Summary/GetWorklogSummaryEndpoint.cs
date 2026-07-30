using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Worklogs.Summary;

public sealed class GetWorklogSummaryEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/work-items/{workItemId:guid}/worklogs/summary",
            async (
                Guid workItemId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new GetWorklogSummaryQuery(workItemId),
                        cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Worklogs")
            .WithName("Worklogs_GetSummary")
            .WithSummary("Get worklog summary")
            .WithDescription("Returns total logged time for a work item.")
            .Produces<GetWorklogSummaryResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }
}
