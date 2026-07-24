using System.Security.Claims;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.MultiFactor.Verify;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints.MultiFactor;

public static class VerifyTwoFactorEndpoint
{
    public static IEndpointRouteBuilder MapVerifyTwoFactorEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/verify",
            [Authorize] async (
                VerifyTwoFactorRequest request,
                ClaimsPrincipal user,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var userIdClaim =
                    user.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    return Results.Unauthorized();
                }

                var command = new VerifyTwoFactorCommand(
                    userId,
                    request.Code);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Two-factor authentication enabled successfully.");
            })
            .WithName("VerifyTwoFactor")
            .WithSummary("Verifies MFA setup")
            .WithDescription("Verifies the authenticator code and enables MFA.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();

        return app;
    }
}

public sealed record VerifyTwoFactorRequest(
    string Code);
