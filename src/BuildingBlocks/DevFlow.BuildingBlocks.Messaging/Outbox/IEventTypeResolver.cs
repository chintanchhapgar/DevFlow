namespace DevFlow.BuildingBlocks.Messaging.Outbox;

/// <summary>
/// Resolves a domain event CLR type from its persisted name.
/// </summary>
public interface IEventTypeResolver
{
    Type Resolve(string eventTypeName);
}
