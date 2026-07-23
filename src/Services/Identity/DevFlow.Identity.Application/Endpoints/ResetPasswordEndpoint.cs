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
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                if (result.IsFailure)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            })
            .AllowAnonymous()
            .WithName("ResetPassword")
            .WithSummary("Reset password")
            .Produces<ResetPasswordResponse>();

        return app;
    }
}
