using DevFlow.Projects.Archive;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Archive;

public sealed record ArchiveProjectCommand(
    Guid ProjectId)
    : IRequest<Result<ArchiveProjectResponse>>;
