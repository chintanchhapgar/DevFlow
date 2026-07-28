using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.MultiFactor.Disable;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Disable;

/// <summary>
/// Disables two-factor authentication.
/// </summary>
internal sealed class DisableTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
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
            .WithTags("MFA")
        .WithName("DisableTwoFactor")
        .WithSummary("Disable two-factor authentication")
        .WithDescription(
            "Disables two-factor authentication after verifying a TOTP or recovery code.")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

    }
}
