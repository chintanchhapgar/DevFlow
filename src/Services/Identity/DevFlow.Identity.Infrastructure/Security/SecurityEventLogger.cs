using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Requests;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Infrastructure.Security;

internal sealed class SecurityEventLogger
    : ISecurityEventLogger
{
    private readonly ISecurityEventRepository _repository;
    private readonly ICurrentRequestInfo _requestInfo;

    public SecurityEventLogger(
        ISecurityEventRepository repository,
        ICurrentRequestInfo requestInfo)
    {
        _repository = repository;
        _requestInfo = requestInfo;
    }

    public async Task LogAsync(
        UserId userId,
        SecurityEventType eventType,
        string? details = null,
        CancellationToken cancellationToken = default)
    {
        var securityEvent = SecurityEvent.Create(
            userId,
            eventType,
            _requestInfo.IpAddress,
            _requestInfo.UserAgent,
            _requestInfo.DeviceName,
            _requestInfo.Browser,
            _requestInfo.OperatingSystem,
            details);

        await _repository.AddAsync(
            securityEvent,
            cancellationToken);
    }
}
