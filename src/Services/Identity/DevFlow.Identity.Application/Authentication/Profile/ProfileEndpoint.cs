using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Authentication.Profile;

/// <summary>
/// Current user endpoint.
/// </summary>
internal sealed class ProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/profile",
            (ICurrentUser currentUser, HttpContext httpContext) =>
            {

                var userId = new UserId(currentUser.UserId);

                var response = new ProfileResponse(
                    new UserId(currentUser.UserId),
                    currentUser.Email,
                    currentUser.Name,
                    currentUser.Role);

                return ApiResponseFactory.Success(
                    httpContext,
                    response,
                    "Profile retrieved successfully.");
            })
            .RequireAuthorization()
                    .WithName("Profile")
                    .WithSummary("Current authenticated user")
                    .Produces<ProfileResponse>();

    }
}
