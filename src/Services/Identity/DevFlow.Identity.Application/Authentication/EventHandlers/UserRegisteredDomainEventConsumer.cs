
using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;
using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Identity.Application.Authentication.Users.DomainEvents;

public sealed class UserRegisteredDomainEventConsumer
    : IDomainEventConsumer<UserRegisteredDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public UserRegisteredDomainEventConsumer(
        IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task ConsumeAsync(
        UserRegisteredDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(
            new UserRegisteredIntegrationEvent(
                domainEvent.UserId.Value,
                domainEvent.Email,
                domainEvent.FirstName,
                domainEvent.LastName,
                domainEvent.VerificationToken),
            cancellationToken);
    }
}
