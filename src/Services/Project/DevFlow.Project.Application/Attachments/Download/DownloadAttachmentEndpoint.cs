using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Security.Authorization;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Attachments.Download;

public sealed class DownloadAttachmentEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/attachments/{attachmentId:guid}",
            async (
                Guid attachmentId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result =
                    await sender.Send(
                        new DownloadAttachmentQuery(
                            attachmentId),
                        cancellationToken);

                if (result.IsFailure)
                {
                    return Results.NotFound();
                }

                return Results.File(
                    result.Value.Content,
                    result.Value.ContentType,
                    result.Value.FileName);
            })
            .WithTags("Attachments")
            .WithName("Attachments_Download")
            .WithSummary("Download attachment")
            .WithDescription("Downloads an attachment by its identifier.")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
