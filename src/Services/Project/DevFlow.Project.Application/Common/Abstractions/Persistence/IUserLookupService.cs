namespace DevFlow.Project.Application.Common.Abstractions.Identity;

public interface IUserLookupService
{
    Task<bool> ExistsAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
