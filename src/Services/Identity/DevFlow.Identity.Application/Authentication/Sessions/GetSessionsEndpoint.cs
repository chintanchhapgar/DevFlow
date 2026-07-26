using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.Sessions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.Sessions;

internal sealed class GetSessionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/sessions",
            async (
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetSessionsQuery(),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .RequireAuthorization()
            .WithName("GetSessions")
            .WithSummary("Get active sessions")
            .Produces<IReadOnlyList<SessionResponse>>(StatusCodes.Status200OK);

    }
}
