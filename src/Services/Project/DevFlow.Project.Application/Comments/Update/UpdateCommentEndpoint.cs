using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Comments.Update;

internal sealed class UpdateCommentEndpoint
{
    public static IEndpointRouteBuilder MapUpdateComment(
        IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/comments/{commentId:guid}",
            async (
                Guid commentId,
                UpdateCommentRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateCommentCommand(
                    commentId,
                    request.Content);

                Result<UpdateCommentResponse> result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
            .WithName("UpdateComment")
            .WithTags("Comments")
            .Produces<Result<UpdateCommentResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(PolicyNames.ProjectEditor);

        return app;
    }
}
