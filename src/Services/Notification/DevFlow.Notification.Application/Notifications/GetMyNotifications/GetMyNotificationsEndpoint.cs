using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using DevFlow.BuildingBlocks.Security.Authorization;
using DevFlow.Notification.Application.Notifications.GetMyNotifications;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Notification.Application.Notifications.GetMyNotifications;

public sealed class GetMyNotificationsEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/api/notifications/{userId:guid}",
            async (
                Guid userId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var query =
                    new GetMyNotificationsQuery(userId);

                var result =
                    await sender.Send(
                        query,
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Notifications")
            .WithName("Notifications_GetMy")
            .WithSummary("Get user notifications")
            .WithDescription("Returns all notifications for the specified user.")
            .Produces<List<NotificationResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
}
