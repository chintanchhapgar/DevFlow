using DevFlow.Project.Domain.Epics.Entities;
using DevFlow.Project.Domain.Epics.ValueObjects;

namespace DevFlow.Project.Domain.Epics.Repositories;

public interface IEpicRepository
{
    Task AddAsync(
        EpicAggregate epic,
        CancellationToken cancellationToken = default);

    Task<EpicAggregate?> GetByIdAsync(
        EpicId id,
        CancellationToken cancellationToken = default);

    Task<EpicAggregate?> GetByNameAsync(
        Guid projectId,
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<EpicAggregate>> GetByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
}
