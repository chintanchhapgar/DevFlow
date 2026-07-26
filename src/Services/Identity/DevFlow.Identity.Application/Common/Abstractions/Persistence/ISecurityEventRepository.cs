using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Common.Abstractions.Persistence;

public interface ISecurityEventRepository
{
    Task AddAsync(
        SecurityEvent securityEvent,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<SecurityEvent>> GetByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken);
}
