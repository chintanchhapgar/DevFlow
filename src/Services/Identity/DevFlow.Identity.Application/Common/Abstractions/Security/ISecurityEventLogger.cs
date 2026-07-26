using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Common.Abstractions.Security;

public interface ISecurityEventLogger
{
    Task LogAsync(
        UserId userId,
        SecurityEventType eventType,
        string? details = null,
        CancellationToken cancellationToken = default);
}
