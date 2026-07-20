using Microsoft.AspNetCore.Routing;

namespace DevFlow.BuildingBlocks.Api.Endpoints;

/// <summary>
/// Contract for minimal API endpoint registration.
/// Each feature implements this to register its routes.
/// </summary>
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
