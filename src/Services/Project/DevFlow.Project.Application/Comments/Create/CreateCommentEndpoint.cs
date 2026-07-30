using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Comments.Create;

internal sealed class CreateCommentEndpoint
{
    public static IEndpointRouteBuilder MapCreateComment(
        IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/workitems/{workItemId:guid}/comments",
            async (
                Guid workItemId,
                CreateCommentRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new CreateCommentCommand(
                    workItemId,
                    request.Content);

                Result<CreateCommentResponse> result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
            .WithName("CreateComment")
            .WithTags("Comments")
            .Produces<Result<CreateCommentResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(PolicyNames.ProjectEditor);
        return app;
    }
}
