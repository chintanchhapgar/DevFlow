using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Restore;

internal sealed class RestoreProjectCommandHandler
    : IRequestHandler<RestoreProjectCommand, Result<RestoreProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RestoreProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<RestoreProjectResponse>> Handle(
        RestoreProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<RestoreProjectResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<RestoreProjectResponse>(
                ProjectErrors.Forbidden);
        }

        if (project.Status == Domain.Projects.Enums.ProjectStatus.Active)
        {
            return Result.Failure<RestoreProjectResponse>(
                ProjectErrors.AlreadyActive);
        }

        project.Restore();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new RestoreProjectResponse(
                project.Id.Value,
                project.Key,
                project.Name,
                project.Status.ToString()),
            "Project restored successfully.");
    }
}
