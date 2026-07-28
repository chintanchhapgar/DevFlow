using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Invitations.Invite;

public sealed record InviteProjectMemberCommand(
    Guid ProjectId,
    string Email,
    ProjectRole Role)
    : IRequest<Result<InviteProjectMemberResponse>>;
