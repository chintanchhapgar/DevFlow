using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.Common;
using DevFlow.Identity.Application.Authentication.MultiFactor.Login;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

/// <summary>
/// Completes two-factor authentication login.
/// </summary>
public static class CompleteTwoFactorLoginEndpoint
{
    public static IEndpointRouteBuilder MapCompleteTwoFactorLoginEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/login",
            async (
                CompleteTwoFactorLoginCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
        .WithName("CompleteTwoFactorLogin")
        .WithSummary("Complete two-factor authentication login")
        .WithDescription(
            "Verifies a TOTP or recovery code and returns JWT tokens.")
        .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .AllowAnonymous();

        return app;
    }
}
