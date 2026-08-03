using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.Identity.Application.Authentication.Users.IntegrationEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Application.Authentication.Users.Events.Consumers;

internal sealed class UserRegisteredConsumer
    : IDomainEventConsumer<UserRegisteredDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public UserRegisteredConsumer(
        IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task ConsumeAsync(
        UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        await _publisher.PublishAsync(
            new UserRegisteredIntegrationEvent(
                domainEvent.UserId.Value,
                domainEvent.Email,
                domainEvent.FirstName,
                domainEvent.LastName),
            cancellationToken);
    }
}
