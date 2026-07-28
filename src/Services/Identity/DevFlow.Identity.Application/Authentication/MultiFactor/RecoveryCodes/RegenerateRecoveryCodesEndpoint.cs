using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.MultiFactor.RecoveryCodes;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.MultiFactor.RecoveryCodes;
/// <summary>
/// Regenerates MFA recovery codes.
/// </summary>
internal sealed class RegenerateRecoveryCodesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/mfa/recovery-codes/regenerate",
            async (
                RegenerateRecoveryCodesCommand command,
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
        .WithName("RegenerateRecoveryCodes")
        .WithSummary("Regenerate MFA recovery codes")
        .WithDescription(
            "Verifies the current TOTP or recovery code and generates a new set of recovery codes.")
        .Produces<RegenerateRecoveryCodesResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .RequireAuthorization();

    }
}
