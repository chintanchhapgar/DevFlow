using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Members.Remove;

public sealed class RemoveProjectMemberEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/projects/{projectId:guid}/members/{userId:guid}",
            async (
                Guid projectId,
                Guid userId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new RemoveProjectMemberCommand(
                    projectId,
                    userId);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithName("RemoveProjectMember")
            .WithSummary("Remove project member")
            .WithDescription("Removes a member from the project. Only the project owner can perform this operation.")
            .Produces<RemoveProjectMemberResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
