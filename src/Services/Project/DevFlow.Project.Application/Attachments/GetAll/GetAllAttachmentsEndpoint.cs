using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Attachments.GetAll;

public sealed class GetAllAttachmentsEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/work-items/{workItemId:guid}/attachments",
            async (
                Guid workItemId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetAllAttachmentsQuery(workItemId),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Attachments")
            .WithName("Attachments_GetAll")
            .WithSummary("Get work item attachments")
            .WithDescription("Returns all attachments for the specified work item.")
            .Produces<IReadOnlyList<GetAllAttachmentsResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
