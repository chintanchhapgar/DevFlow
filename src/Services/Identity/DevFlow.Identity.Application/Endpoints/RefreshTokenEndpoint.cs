using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

public static class RefreshTokenEndpoint
{
    public static IEndpointRouteBuilder MapRefreshTokenEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/refresh",
            async (
                RefreshTokenCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Token refreshed successfully.");
            })
            .AllowAnonymous()
            .WithName("RefreshToken")
            .WithSummary("Refresh expired access token")
            .Produces<RefreshTokenResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest);

        return app;
    }
}
