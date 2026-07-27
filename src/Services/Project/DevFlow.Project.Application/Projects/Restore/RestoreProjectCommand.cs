using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Restore;

public sealed record RestoreProjectCommand(
    Guid ProjectId)
    : IRequest<Result<RestoreProjectResponse>>;
