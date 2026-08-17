using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.UpdateRole;

public sealed record UpdateUserRoleCommand(
    Guid UserId,
    UserRole Role)
    : IRequest<Result<UpdateUserRoleResponse>>;
