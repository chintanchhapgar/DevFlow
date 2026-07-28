using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.Project.Domain.Projects.ValueObjects;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Invite;

internal sealed class InviteProjectMemberCommandHandler
    : IRequestHandler<
        InviteProjectMemberCommand,
        Result<InviteProjectMemberResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public InviteProjectMemberCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<InviteProjectMemberResponse>> Handle(
        InviteProjectMemberCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(
            new ProjectId(request.ProjectId),
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<InviteProjectMemberResponse>(
                ProjectErrors.NotFound);
        }

        if (project.OwnerId != _currentUser.UserId)
        {
            return Result.Failure<InviteProjectMemberResponse>(
                ProjectErrors.Forbidden);
        }

        var result = project.InviteMember(
            request.Email,
            request.Role,
            _currentUser.UserId);

        if (result.IsFailure)
        {
            return Result.Failure<InviteProjectMemberResponse>(
                result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var invitation = result.Value;

        return Result.Success(
            new InviteProjectMemberResponse(
                invitation.Id,
                project.Id.Value,
                invitation.Email,
                invitation.Role.ToString(),
                invitation.Token),
            "Invitation created successfully.");
    }
}
