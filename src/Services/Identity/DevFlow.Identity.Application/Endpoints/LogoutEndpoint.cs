using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.Logout;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

public static class LogoutEndpoint
{
    public static IEndpointRouteBuilder MapLogoutEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/logout",
            async (
                LogoutCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .RequireAuthorization()
            .WithName("Logout")
            .WithSummary("Logout current user")
            .Produces<LogoutResponse>();

        return app;
    }
}
