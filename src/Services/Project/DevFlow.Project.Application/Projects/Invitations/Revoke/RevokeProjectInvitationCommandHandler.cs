using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Revoke;

internal sealed class RevokeProjectInvitationCommandHandler
    : IRequestHandler<
        RevokeProjectInvitationCommand,
        Result<RevokeProjectInvitationResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public RevokeProjectInvitationCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<RevokeProjectInvitationResponse>> Handle(
        RevokeProjectInvitationCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<RevokeProjectInvitationResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<RevokeProjectInvitationResponse>(
                ProjectErrors.Forbidden);
        }

        var result = project.RevokeInvitation(
            request.InvitationId);

        if (result.IsFailure)
        {
            return Result.Failure<RevokeProjectInvitationResponse>(
                result.Error);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new RevokeProjectInvitationResponse(
                project.Id.Value,
                request.InvitationId,
                "Revoked"),
            "Invitation revoked successfully.");
    }
}
