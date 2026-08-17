using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Identity.Application.Users.GetAll;

public sealed class GetAllUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/users",
            async (
                int page,
                int pageSize,
                string? search,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetAllUsersQuery(
                        page <= 0 ? 1 : page,
                        pageSize <= 0 ? 20 : pageSize,
                        search),
                    cancellationToken);

                return result.ToApiResult(httpContext);
            })
        .WithTags("Users")
        .WithName("GetAllUsers")
        .WithSummary("Get users")
        .WithDescription("Returns paged users with optional search.")
        .Produces<GetAllUsersResponse>(StatusCodes.Status200OK)
        .RequireAuthorization("UserRoleManager");
    }
}
