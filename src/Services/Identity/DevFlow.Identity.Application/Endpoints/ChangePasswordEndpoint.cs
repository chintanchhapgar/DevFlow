using DevFlow.Identity.Application.Authentication.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

/// <summary>
/// Change password endpoint.
/// </summary>
public static class ChangePasswordEndpoint
{
    public static IEndpointRouteBuilder MapChangePasswordEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/change-password",
            async (
                ChangePasswordCommand command,
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
            .RequireAuthorization()
            .WithName("ChangePassword")
            .WithSummary("Change current user's password")
            .Produces<ChangePasswordResponse>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}
