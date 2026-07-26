using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeSession;

internal sealed class RevokeSessionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/auth/sessions/{sessionId:guid}",
            async (
                Guid sessionId,
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new RevokeSessionCommand(sessionId),
                    cancellationToken);

                return result.ToApiResult(
                    context,
                    "Session revoked.");
            })
            .RequireAuthorization()
            .WithName("RevokeSession")
            .WithTags("Sessions");
    }
}
