using DevFlow.Project.Domain.Projects.Entities;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Pagination;

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

    Task<PagedList<ProjectAggregate>> GetPagedAsync(
        PaginationRequest pagination,
        string? search,
        Guid? memberId,
        CancellationToken cancellationToken = default);

    Task<ProjectAggregate?> GetByInvitationTokenAsync(
        Guid token,
        CancellationToken cancellationToken = default);
}
