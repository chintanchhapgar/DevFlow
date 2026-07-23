using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

/// <summary>
/// Change password endpoint.
/// </summary>
public static class ChangePasswordEndpoint
{
    public static IEndpointRouteBuilder MapChangePasswordEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/change-password",
            async (
                ChangePasswordCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Password changed successfully.");
            })
            .RequireAuthorization()
            .WithName("ChangePassword")
            .WithSummary("Change current user's password")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden);

        return app;
    }
}
