using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.LogoutAll;

internal sealed class LogoutAllEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "/api/auth/logout-all",
                async (
                    LogoutAllCommand command,
                    ISender sender,
                    HttpContext httpContext,
                    CancellationToken cancellationToken) =>
                {
                    var result = await sender.Send(
                        command,
                        cancellationToken);

                    return result.ToApiResult(httpContext);
                })
            .WithTags("Auth")
            .RequireAuthorization()
            .WithName("LogoutAll")
            .WithSummary("Logout from all active sessions")
            .Produces<LogoutAllResponse>()
            .Produces(StatusCodes.Status401Unauthorized);
    }
}
