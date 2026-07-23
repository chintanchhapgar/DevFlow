using DevFlow.BuildingBlocks.Api.Responses;
using DevFlow.Identity.Application.Authentication.Profile;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.API.Endpoints;

/// <summary>
/// Current user endpoint.
/// </summary>
public static class ProfileEndpoint
{
    public static IEndpointRouteBuilder MapProfileEndpoint(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/auth/profile",
            (ICurrentUser currentUser, HttpContext httpContext) =>
            {
                var response = new ProfileResponse(
                    currentUser.UserId,
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

        return app;
    }
}
