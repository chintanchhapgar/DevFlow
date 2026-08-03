using Microsoft.Extensions.Logging;

namespace DevFlow.Identity.Application.Authentication.Users.Events.Consumers;

internal static partial class UserRegisteredConsumerLog
{
    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "User registered. UserId: {UserId}, Email: {Email}")]
    internal static partial void UserRegistered(
        ILogger logger,
        Guid userId,
        string email);
}
