using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Members.UpdateRole;

public sealed class UpdateProjectMemberRoleEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPatch(
            "/api/projects/{projectId:guid}/members/{userId:guid}/role",
            async (
                Guid projectId,
                Guid userId,
                UpdateProjectMemberRoleRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdateProjectMemberRoleCommand(
                    projectId,
                    userId,
                    request.Role);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithName("UpdateProjectMemberRole")
            .WithSummary("Update project member role")
            .WithDescription("Updates the role of a project member. Only the project owner can perform this operation.")
            .Produces<UpdateProjectMemberRoleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequireAuthorization();
    }
}
