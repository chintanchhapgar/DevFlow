namespace DevFlow.Project.Application.Common.Abstractions.Persistence;

using DevFlow.Project.Application.Reports.Burndown;

public interface IBurndownRepository
{
    Task<GetBurndownResponse?> GetAsync(
        Guid sprintId,
        CancellationToken cancellationToken = default);
}
