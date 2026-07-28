using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Users.Exists;

public sealed class UserExistsEndpoint : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/users/{userId:guid}/exists",
            async (
                Guid userId,
                IUserRepository repository,
                CancellationToken cancellationToken) =>
            {
                var exists = await repository.ExistsByIdAsync(
                    userId,
                    cancellationToken);

                return exists
                    ? Results.Ok()
                    : Results.NotFound();
            })
        .WithName("UserExists")
        .WithSummary("Checks whether a user exists.");
    }
}
