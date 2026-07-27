using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Update;

internal sealed class UpdateProjectCommandHandler
    : IRequestHandler<UpdateProjectCommand, Result<UpdateProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateProjectCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<UpdateProjectResponse>> Handle(
        UpdateProjectCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<UpdateProjectResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<UpdateProjectResponse>(
                ProjectErrors.Forbidden);
        }

        project.Update(
            request.Name,
            request.Description,
            request.Visibility);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateProjectResponse(
                project.Id.Value,
                project.Key,
                project.Name),
            "Project updated successfully.");
    }
}
