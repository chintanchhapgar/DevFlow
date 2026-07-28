using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Exceptions;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.UpdateRole;

internal sealed class UpdateProjectMemberRoleCommandHandler
    : IRequestHandler<
        UpdateProjectMemberRoleCommand,
        Result<UpdateProjectMemberRoleResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public UpdateProjectMemberRoleCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<UpdateProjectMemberRoleResponse>> Handle(
        UpdateProjectMemberRoleCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<UpdateProjectMemberRoleResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<UpdateProjectMemberRoleResponse>(
                ProjectErrors.Forbidden);
        }

        try
        {
            project.ChangeMemberRole(
                request.UserId,
                request.Role);
        }
        catch (DomainException ex)
        {
            return Result.Failure<UpdateProjectMemberRoleResponse>(
                ex.AppError);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new UpdateProjectMemberRoleResponse(
                project.Id.Value,
                request.UserId,
                request.Role.ToString()),
            "Project member role updated successfully.");
    }
}
