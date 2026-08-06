using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Projects;
using DevFlow.BuildingBlocks.Messaging.IntegrationEvents;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects;
using DevFlow.Project.Domain.Projects.Events;
using DevFlow.SharedKernel.Domain.DomainEvents;

namespace DevFlow.Project.Application.Projects.DomainEvents;

public sealed class ProjectCreatedDomainEventConsumer
    : IDomainEventConsumer<ProjectCreatedDomainEvent>
{
    private readonly IIntegrationEventPublisher _publisher;

    public ProjectCreatedDomainEventConsumer(
        IIntegrationEventPublisher publisher)
    {
        _publisher = publisher;
    }

    public async Task ConsumeAsync(
        ProjectCreatedDomainEvent domainEvent,
        CancellationToken cancellationToken = default)
    {
        await _publisher.PublishAsync(
            new ProjectCreatedIntegrationEvent(
                domainEvent.ProjectId.Value,
                domainEvent.OwnerId,
                domainEvent.Name,
                domainEvent.Description),
            cancellationToken);
    }
}
