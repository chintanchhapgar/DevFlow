using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Setup;

internal sealed class SetupTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/setup",
            [Authorize] async (
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new SetupTwoFactorCommand(),
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Two-factor authentication setup initialized.");
            })
            .WithTags("MFA")
            .WithName("SetupTwoFactor")
            .WithSummary("Starts MFA setup")
            .WithDescription(
                "Generates a TOTP secret and QR code URI for authenticator apps.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }
}
