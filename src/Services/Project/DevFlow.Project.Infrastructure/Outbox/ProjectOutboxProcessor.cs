using DevFlow.BuildingBlocks.Messaging.Outbox;
using DevFlow.BuildingBlocks.Messaging.Serialization;
using DevFlow.Project.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevFlow.Project.Infrastructure.Outbox;

internal sealed class ProjectOutboxProcessor
    : OutboxProcessor<ProjectDbContext>
{
    public ProjectOutboxProcessor(
        IServiceScopeFactory scopeFactory,
        ILogger<ProjectOutboxProcessor> logger,
        IMessageSerializer serializer,
        IIntegrationEventTypeResolver eventTypeResolver)
        : base(
            scopeFactory,
            logger,
            serializer,
            eventTypeResolver)
    {   
    }

    protected override async Task SaveChangesAsync(
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken)
    {
        var dbContext =
            serviceProvider.GetRequiredService<ProjectDbContext>();

        await dbContext.SaveChangesAsync(
            cancellationToken);
    }
}
