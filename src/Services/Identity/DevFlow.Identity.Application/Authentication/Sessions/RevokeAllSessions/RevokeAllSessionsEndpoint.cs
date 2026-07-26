using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.Sessions.RevokeAllSessions;

internal sealed class RevokeAllSessionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/api/auth/sessions",
            async (
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new RevokeAllSessionsCommand(),
                    cancellationToken);

                return result.ToApiResult(
                    context,
                    "All sessions revoked successfully.");
            })
            .RequireAuthorization()
            .WithTags("Sessions")
            .WithName("RevokeAllSessions");
    }
}
