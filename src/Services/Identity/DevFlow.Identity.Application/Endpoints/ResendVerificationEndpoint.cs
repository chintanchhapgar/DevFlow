using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.ResendVerification;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

/// <summary>
/// Resend email verification endpoint.
/// </summary>
public static class ResendVerificationEndpoint
{
    public static IEndpointRouteBuilder MapResendVerificationEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/resend-verification",
            async (
                ResendVerificationCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .AllowAnonymous()
            .WithName("ResendVerification")
            .WithSummary("Resend email verification")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        return app;
    }
}
