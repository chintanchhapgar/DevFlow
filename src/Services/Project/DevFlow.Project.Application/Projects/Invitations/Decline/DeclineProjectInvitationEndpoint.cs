using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Project.Application.Projects.Invitations.Decline;

public sealed class DeclineProjectInvitationEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/projects/invitations/decline",
            async (
                DeclineProjectInvitationRequest request,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var command = new DeclineProjectInvitationCommand(
                    request.Token);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .WithTags("Project Invitations")
            .WithName("DeclineProjectInvitation")
            .WithSummary("Decline project invitation")
            .WithDescription("Declines a pending invitation.")
            .Produces<DeclineProjectInvitationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .RequireAuthorization();
    }
}
