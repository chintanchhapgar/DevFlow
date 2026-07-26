using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeOtherSessions;

internal sealed class RevokeOtherSessionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/auth/sessions/others",
            async (
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new RevokeOtherSessionsCommand(),
                    cancellationToken);

                return result.ToApiResult(
                    context,
                    "Other sessions revoked successfully.");
            })
            .RequireAuthorization()
            .WithName("RevokeOtherSessions")
            .WithTags("Sessions");
    }
}
