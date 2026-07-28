using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Accept;

internal sealed class AcceptProjectInvitationCommandHandler
    : IRequestHandler<
        AcceptProjectInvitationCommand,
        Result<AcceptProjectInvitationResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public AcceptProjectInvitationCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<AcceptProjectInvitationResponse>> Handle(
        AcceptProjectInvitationCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByInvitationTokenAsync(
            request.Token,
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<AcceptProjectInvitationResponse>(
                ProjectErrors.InvitationNotFound);
        }

        var invitation = project.Invitations
            .First(x => x.Token == request.Token);

        var result = project.AcceptInvitation(
            request.Token,
            _currentUser.UserId);

        if (result.IsFailure)
        {
            return Result.Failure<AcceptProjectInvitationResponse>(
                result.Error);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new AcceptProjectInvitationResponse(
                project.Id.Value,
                invitation.Id,
                _currentUser.UserId,
                invitation.Role.ToString()),
            "Invitation accepted successfully.");
    }
}
