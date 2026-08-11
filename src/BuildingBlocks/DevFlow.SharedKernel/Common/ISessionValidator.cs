namespace DevFlow.SharedKernel.Common;

public interface ISessionValidator
{
    Task<bool> IsSessionActiveAsync(
        Guid userId,
        Guid sessionId,
        CancellationToken cancellationToken = default);
}
