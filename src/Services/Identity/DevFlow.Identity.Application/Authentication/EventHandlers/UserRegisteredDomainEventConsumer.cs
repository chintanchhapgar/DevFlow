
using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;
using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Application.Authentication.Users.DomainEvents;

public sealed class UserRegisteredDomainEventConsumer
    : IDomainEventConsumer<UserRegisteredDomainEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IIntegrationEventPublisher _publisher;

    public UserRegisteredDomainEventConsumer(
        IUserRepository userRepository,
        IIntegrationEventPublisher publisher)
    {
        _userRepository = userRepository;
        _publisher = publisher;
    }

    public async Task ConsumeAsync(
        UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            domainEvent.UserId,
            cancellationToken);

        if (user is null)
        {
            return;
        }

        await _publisher.PublishAsync(
            new UserRegisteredIntegrationEvent(
                user.Id.Value,
                user.Email,
                user.FirstName,
                user.LastName),
            cancellationToken);
    }
}
