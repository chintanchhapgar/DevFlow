using DevFlow.Project.Application.Reports.Velocity;

namespace DevFlow.Project.Application.Common.Abstractions.Persistence;

public interface IVelocityRepository
{
    Task<GetVelocityResponse?> GetAsync(
        Guid projectId,
        CancellationToken cancellationToken = default);
}
