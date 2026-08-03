using System.Text.Json;

namespace DevFlow.BuildingBlocks.Messaging.Serialization;

/// <summary>
/// JSON implementation of the outbox serializer.
/// </summary>
public sealed class JsonMessageSerializer : IMessageSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    public string Serialize<T>(T message)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(message);

        return JsonSerializer.Serialize(
            message,
            message.GetType(),
            SerializerOptions);
    }

    public T Deserialize<T>(string content)
        where T : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);

        return JsonSerializer.Deserialize<T>(
                   content,
                   SerializerOptions)
               ?? throw new InvalidOperationException(
                   $"Unable to deserialize '{typeof(T).FullName}'.");
    }

    public object Deserialize(
        string content,
        Type messageType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(content);
        ArgumentNullException.ThrowIfNull(messageType);

        return JsonSerializer.Deserialize(
                   content,
                   messageType,
                   SerializerOptions)
               ?? throw new InvalidOperationException(
                   $"Unable to deserialize '{messageType.FullName}'.");
    }
}
