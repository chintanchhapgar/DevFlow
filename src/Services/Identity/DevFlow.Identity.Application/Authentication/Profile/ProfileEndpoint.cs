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
            async (
                ICurrentUser currentUser,
                IUserRepository users,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var userId = new UserId(currentUser.UserId);
                var user = await users.GetByIdAsync(
                    userId,
                    cancellationToken);

                if (user is null)
                {
                    return Results.NotFound();
                }

                var response = new ProfileResponse(
                    userId,
                    currentUser.Email,
                    currentUser.Name,
                    currentUser.Role,
                    user.IsTwoFactorEnabled);

                return ApiResponseFactory.Success(
                    httpContext,
                    response,
                    "Profile retrieved successfully.");
            })
            .WithTags("User")
            .RequireAuthorization()
                    .WithName("Profile")
                    .WithSummary("Current authenticated user")
                    .Produces<ProfileResponse>();

    }
}
