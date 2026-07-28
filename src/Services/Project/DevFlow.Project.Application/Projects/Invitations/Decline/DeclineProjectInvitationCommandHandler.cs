using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.SharedKernel.Common;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Decline;

internal sealed class DeclineProjectInvitationCommandHandler
    : IRequestHandler<
        DeclineProjectInvitationCommand,
        Result<DeclineProjectInvitationResponse>>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public DeclineProjectInvitationCommandHandler(
        IProjectRepository projectRepository,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser)
    {
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<DeclineProjectInvitationResponse>> Handle(
        DeclineProjectInvitationCommand request,
        CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByInvitationTokenAsync(
            request.Token,
            cancellationToken);

        if (project is null)
        {
            return Result.Failure<DeclineProjectInvitationResponse>(
                ProjectErrors.InvitationNotFound);
        }

        var invitation = project.Invitations
            .First(x => x.Token == request.Token);

        var result = project.DeclineInvitation(
            request.Token);

        if (result.IsFailure)
        {
            return Result.Failure<DeclineProjectInvitationResponse>(
                result.Error);
        }

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success(
            new DeclineProjectInvitationResponse(
                project.Id.Value,
                invitation.Id,
                "Declined"),
            "Invitation declined successfully.");
    }
}
