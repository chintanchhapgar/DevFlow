using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.Projects.Archive;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Archive;

internal sealed class ArchiveProjectCommandHandler
    : IRequestHandler<ArchiveProjectCommand, Result<ArchiveProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public ArchiveProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<ArchiveProjectResponse>> Handle(
        ArchiveProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<ArchiveProjectResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<ArchiveProjectResponse>(
                ProjectErrors.Forbidden);
        }

        if (project.Status == Domain.Projects.Enums.ProjectStatus.Archived)
        {
            return Result.Failure<ArchiveProjectResponse>(
                ProjectErrors.AlreadyArchived);
        }

        project.Archive();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new ArchiveProjectResponse(
                project.Id.Value,
                project.Key,
                project.Name,
                project.Status.ToString()),
            "Project archived successfully.");
    }
}
