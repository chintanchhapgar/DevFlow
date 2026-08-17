using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.Identity.Domain.Authentication.Users;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Users.UpdateRole;

public sealed class UpdateUserRoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/api/users/{userId:guid}/role",
            async (
                Guid userId,
                UpdateUserRoleRequest request,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new UpdateUserRoleCommand(userId, request.Role),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
        .WithTags("Users")
        .WithName("UpdateUserRole")
        .WithSummary("Update a user's system role")
        .Produces<UpdateUserRoleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status403Forbidden)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireAuthorization("UserRoleManager");
    }
}

public sealed record UpdateUserRoleRequest(UserRole Role);
