using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Attachments.Upload;

public sealed class UploadAttachmentEndpoint
    : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/work-items/{workItemId:guid}/attachments",
            async (
                Guid workItemId,
                IFormFile file, // <-- Pass IFormFile directly here
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new UploadAttachmentCommand(workItemId, file);

                var result = await sender.Send(command, cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .DisableAntiforgery()
            // REMOVE the .Accepts() line entirely. Swagger figures it out automatically.
            .WithTags("Attachments")
            .WithName("Attachments_Upload")
            .WithSummary("Upload attachment")
            .WithDescription("Uploads a file to a work item.")
            .Produces<UploadAttachmentResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization(PolicyNames.ProjectEditor);
    }
}
