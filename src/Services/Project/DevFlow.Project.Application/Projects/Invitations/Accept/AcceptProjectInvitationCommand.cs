using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Accept;

public sealed record AcceptProjectInvitationCommand(
    Guid Token)
    : IRequest<Result<AcceptProjectInvitationResponse>>;
