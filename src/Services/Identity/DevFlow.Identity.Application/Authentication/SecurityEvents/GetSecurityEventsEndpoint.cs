using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Application.Authentication.SecurityEvents;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Api.Authentication.SecurityEvents;

internal sealed class GetSecurityEventsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/security-events",
            async (
                ISender sender,
                HttpContext context,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetSecurityEventsQuery(),
                    cancellationToken);

                return result.ToApiResult(context);
            })
            .RequireAuthorization()
            .WithName("GetSecurityEvents")
            .WithTags("Security");
    }
}
