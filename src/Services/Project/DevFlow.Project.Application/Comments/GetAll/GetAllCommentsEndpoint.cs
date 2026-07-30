using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.SharedKernel.Results;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Comments.GetAll;

internal sealed class GetAllCommentsEndpoint
{
    public static IEndpointRouteBuilder MapGetAllComments(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/workitems/{workItemId:guid}/comments",
            async (
                Guid workItemId,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var query = new GetAllCommentsQuery(workItemId);

                Result<IReadOnlyList<GetAllCommentsResponse>> result =
                    await sender.Send(
                        query,
                        cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result)
                    : Results.BadRequest(result);
            })
            .WithName("GetAllComments")
            .WithTags("Comments")
            .Produces<Result<IReadOnlyList<GetAllCommentsResponse>>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(PolicyNames.ProjectViewer);

        return app;
    }
}
