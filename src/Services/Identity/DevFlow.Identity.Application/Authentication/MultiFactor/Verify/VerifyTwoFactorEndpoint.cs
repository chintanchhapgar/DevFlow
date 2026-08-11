using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.Verify;

internal sealed class VerifyTwoFactorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/verify",
            [Authorize] async (
                VerifyTwoFactorRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command = new VerifyTwoFactorCommand(
                    request.Code);

                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Two-factor authentication enabled successfully.");
            })
            .WithTags("MFA")
            .WithName("VerifyTwoFactor")
            .WithSummary("Verifies MFA setup")
            .WithDescription(
                "Verifies the authenticator code and enables MFA.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }
}

public sealed record VerifyTwoFactorRequest(
    string Code);
