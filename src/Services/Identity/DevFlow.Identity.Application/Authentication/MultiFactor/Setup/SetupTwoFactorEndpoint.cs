using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Setup;

internal sealed class SetupTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/setup",
            [Authorize] async (
                ClaimsPrincipal user,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                var command = new SetupTwoFactorCommand(userId);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Two-factor authentication setup initialized.");
            })
            .WithTags("MFA")
            .WithName("SetupTwoFactor")
            .WithSummary("Starts MFA setup")
            .WithDescription("Generates a TOTP secret and QR code URI for authenticator apps.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

    }
}
