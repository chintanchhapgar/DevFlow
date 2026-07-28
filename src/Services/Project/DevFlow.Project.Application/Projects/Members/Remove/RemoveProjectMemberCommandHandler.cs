using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Exceptions;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.Remove;

internal sealed class RemoveProjectMemberCommandHandler
    : IRequestHandler<
        RemoveProjectMemberCommand,
        Result<RemoveProjectMemberResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RemoveProjectMemberCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<RemoveProjectMemberResponse>> Handle(
        RemoveProjectMemberCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<RemoveProjectMemberResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<RemoveProjectMemberResponse>(
                ProjectErrors.Forbidden);
        }

        try
        {
            project.RemoveMember(request.UserId);
        }
        catch (DomainException ex)
        {
            return Result.Failure<RemoveProjectMemberResponse>(
                ex.AppError);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new RemoveProjectMemberResponse(
                project.Id.Value,
                request.UserId),
            "Project member removed successfully.");
    }
}
