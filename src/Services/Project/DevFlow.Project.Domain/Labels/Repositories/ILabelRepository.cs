using DevFlow.Project.Domain.Labels.Entities;
using DevFlow.Project.Domain.Labels.ValueObjects;

namespace DevFlow.Project.Domain.Labels.Repositories;

public interface ILabelRepository
{
    Task AddAsync(
        Label label,
        CancellationToken cancellationToken = default);

    Task<Label?> GetByIdAsync(
        LabelId id,
        CancellationToken cancellationToken = default);

    Task<Label?> GetByNameAsync(
        Guid projectId,
        string name,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Label>> GetByProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
}
