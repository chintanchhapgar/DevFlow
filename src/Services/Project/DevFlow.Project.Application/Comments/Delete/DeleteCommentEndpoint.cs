using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Comments.Delete;

internal sealed class DeleteCommentEndpoint
{
    public static IEndpointRouteBuilder MapDeleteComment(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/comments/{commentId:guid}",
            async (
                Guid commentId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new DeleteCommentCommand(commentId);

                Result<DeleteCommentResponse> result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
            .WithName("DeleteComment")
            .WithTags("Comments")
            .Produces<Result<DeleteCommentResponse>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}
