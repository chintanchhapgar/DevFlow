using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Attachments.Delete;

public sealed class DeleteAttachmentEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/attachments/{attachmentId:guid}",
            async (
                Guid attachmentId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new DeleteAttachmentCommand(
                            attachmentId),
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Attachments")
            .WithName("Attachments_Delete")
            .WithSummary("Delete attachment")
            .WithDescription("Soft deletes an attachment and removes the physical file.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
