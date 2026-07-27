using DevFlow.Project.Domain.Projects.Entities;
using DevFlow.Project.Domain.Projects.ValueObjects;

namespace DevFlow.Project.Application.Common.Abstractions.Persistence;

public interface IProjectRepository
{
    Task<ProjectAggregate?> GetByIdAsync(
        ProjectId id,
        CancellationToken cancellationToken);

    Task<ProjectAggregate?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken);

    Task<bool> ExistsByKeyAsync(
        string key,
        CancellationToken cancellationToken);

    Task AddAsync(
        ProjectAggregate project,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ProjectAggregate project,
        CancellationToken cancellationToken);
}
