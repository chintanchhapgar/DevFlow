using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

public static class ResetPasswordEndpoint
{
    public static IEndpointRouteBuilder MapResetPasswordEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/reset-password",
            async (
                ResetPasswordCommand command,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.ToApiResult(
                    httpContext,
                    "Password reset successfully.");
            })
            .AllowAnonymous()
            .WithName("ResetPassword")
            .WithSummary("Reset password")
            .Produces<ResetPasswordResponse>();

        return app;
    }
}
