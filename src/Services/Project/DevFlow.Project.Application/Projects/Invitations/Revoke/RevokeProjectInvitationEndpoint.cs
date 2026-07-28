using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Invitations.Revoke;

public sealed class RevokeProjectInvitationEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/projects/{projectId:guid}/invitations/{invitationId:guid}",
            async (
                Guid projectId,
                Guid invitationId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new RevokeProjectInvitationCommand(
                    projectId,
                    invitationId);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Project Invitations")
            .WithName("RevokeProjectInvitation")
            .WithSummary("Revoke project invitation")
            .WithDescription("Revokes a pending project invitation.")
            .Produces<RevokeProjectInvitationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }
}
