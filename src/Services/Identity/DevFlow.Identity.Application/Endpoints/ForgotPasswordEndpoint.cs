using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.ForgotPassword;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

public static class ForgotPasswordEndpoint
{
    public static IEndpointRouteBuilder MapForgotPasswordEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/api/auth/forgot-password",
            async (
                ForgotPasswordCommand command,
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
            .WithName("ForgotPassword")
            .WithSummary("Request password reset")
            .Produces<ForgotPasswordResponse>();

        return app;
    }
}
