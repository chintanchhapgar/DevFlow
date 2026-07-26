using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.SecurityEvents;

internal sealed class GetSecurityEventsQueryHandler
    : IRequestHandler<
        GetSecurityEventsQuery,
        Result<IReadOnlyList<SecurityEventResponse>>>
{
    private readonly ISecurityEventRepository _repository;
    private readonly ICurrentUser _currentUser;

    public GetSecurityEventsQueryHandler(
        ISecurityEventRepository repository,
        ICurrentUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyList<SecurityEventResponse>>> Handle(
        GetSecurityEventsQuery request,
        CancellationToken cancellationToken)
    {
        var events = await _repository.GetByUserIdAsync(
            new UserId(_currentUser.UserId),
            cancellationToken);

        var response = events
            .Select(x => new SecurityEventResponse(
                x.Id.Value,
                x.EventType.ToString(),
                x.DeviceName,
                x.Browser,
                x.OperatingSystem,
                x.IpAddress,
                x.Details,
                x.OccurredOnUtc))
            .ToList();

        return Result.Success<IReadOnlyList<SecurityEventResponse>>(
            response);
    }
}
