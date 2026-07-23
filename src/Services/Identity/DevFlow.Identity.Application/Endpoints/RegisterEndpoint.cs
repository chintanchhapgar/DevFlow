using DevFlow.Identity.Application.Authentication.Register;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

public static class RegisterEndpoint
{
    public static IEndpointRouteBuilder MapRegisterEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/register",
            async (
                RegisterCommand command,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    command,
                    cancellationToken);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Error);
            });

        return app;
    }
}
