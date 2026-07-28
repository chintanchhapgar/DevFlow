using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Decline;

public sealed record DeclineProjectInvitationCommand(
    Guid Token)
    : IRequest<Result<DeclineProjectInvitationResponse>>;
