namespace DevFlow.BuildingBlocks.Api.Responses;

/// <summary>
/// Provides a message that should be used as the API success message.
/// </summary>
public interface IApiMessage
{
    string Message { get; }
}
