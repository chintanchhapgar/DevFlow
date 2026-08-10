using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

/// <summary>
/// Email verification endpoint.
/// </summary>
internal sealed class VerifyEmailEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/verify-email",
            async (
                Guid token,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new VerifyEmailCommand(token),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Authentication")
            .AllowAnonymous()
            .WithName("VerifyEmail")
            .WithSummary("Verify user email")
            .Produces<VerifyEmailResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound);
    }
}
