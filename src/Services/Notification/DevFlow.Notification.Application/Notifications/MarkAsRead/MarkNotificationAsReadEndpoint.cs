using DevFlow.BuildingBlocks.Api.Endpoints;
using DevFlow.BuildingBlocks.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace DevFlow.Notification.Application.Notifications.MarkAsRead;

public sealed class MarkNotificationAsReadEndpoint
    : IEndpoint
{
    public void MapEndpoint(
        IEndpointRouteBuilder app)
    {
        app.MapPatch(
            "/api/notifications/{notificationId:guid}/read",
            async (
                Guid notificationId,
                ISender sender,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var command =
                    new MarkNotificationAsReadCommand(
                        notificationId);

                var result =
                    await sender.Send(
                        command,
                        cancellationToken);

                return result.ToApiResult(httpContext);
            })
            .WithTags("Notifications")
            .WithName("Notifications_MarkAsRead")
            .WithSummary("Mark notification as read")
            .WithDescription("Marks a notification as read.")
            .Produces<MarkNotificationAsReadResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }
}
