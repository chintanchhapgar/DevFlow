namespace DevFlow.BuildingBlocks.Messaging.Serialization;

public interface IMessageSerializer
{
    string Serialize<T>(T message)
        where T : class;

    T Deserialize<T>(string content)
        where T : class;

    object Deserialize(
        string content,
        Type messageType);
}
