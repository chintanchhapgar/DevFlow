using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.MultiFactor.Disable;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Endpoints.MultiFactor;

/// <summary>
/// Disables two-factor authentication.
/// </summary>
public static class DisableTwoFactorEndpoint
{
    public static IEndpointRouteBuilder MapDisableTwoFactorEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/disable",
            async (
                DisableTwoFactorCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
        .WithName("DisableTwoFactor")
        .WithSummary("Disable two-factor authentication")
        .WithDescription(
            "Disables two-factor authentication after verifying a TOTP or recovery code.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

        return app;
    }
}
