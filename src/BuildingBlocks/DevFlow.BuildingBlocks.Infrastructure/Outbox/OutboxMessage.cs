namespace DevFlow.BuildingBlocks.Infrastructure.Outbox;

/// <summary>
/// Represents a message stored in the transactional outbox.
/// Guarantees at-least-once delivery of integration events.
/// </summary>
public sealed class OutboxMessage
{
    private OutboxMessage()
    {
    }

    public Guid Id { get; private set; }

    public string Type { get; private set; } = string.Empty;

    public string Content { get; private set; } = string.Empty;

    public DateTime CreatedOnUtc { get; private set; }

    public DateTime? ProcessedOnUtc { get; private set; }

    public string? Error { get; private set; }

    public int RetryCount { get; private set; }

    public bool IsProcessed => ProcessedOnUtc.HasValue;

    public static OutboxMessage Create(
        string type,
        string content,
        DateTime createdOnUtc)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(type);
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = type,
            Content = content,
            CreatedOnUtc = createdOnUtc,
            RetryCount = 0
        };
    }

    public void MarkAsProcessed(DateTime processedOnUtc)
    {
        ProcessedOnUtc = processedOnUtc;
        Error = null;
    }

    public void MarkAsFailed(string error)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(error);

        RetryCount++;
        Error = error;
    }
}
