using DevFlow.Project.Domain.Projects.Enums;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Projects.Members.UpdateRole;

public sealed record UpdateProjectMemberRoleCommand(
    Guid ProjectId,
    Guid UserId,
    ProjectRole Role)
    : IRequest<Result<UpdateProjectMemberRoleResponse>>;
