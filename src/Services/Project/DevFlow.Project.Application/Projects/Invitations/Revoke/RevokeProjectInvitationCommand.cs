using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Revoke;

public sealed record RevokeProjectInvitationCommand(
    Guid ProjectId,
    Guid InvitationId)
    : IRequest<Result<RevokeProjectInvitationResponse>>;
