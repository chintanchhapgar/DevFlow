using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.Identity.Domain.Authentication.Users.ValueObjects;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.UpdateRole;

internal sealed class UpdateUserRoleCommandHandler
    : IRequestHandler<
        UpdateUserRoleCommand,
        Result<UpdateUserRoleResponse>>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserRoleCommandHandler(
        IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UpdateUserRoleResponse>> Handle(
        UpdateUserRoleCommand request,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(request.Role))
        {
            return Result.Failure<UpdateUserRoleResponse>(
                UserErrors.InvalidRole);
        }

        var user = await _userRepository.GetByIdAsync(
            new UserId(request.UserId),
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<UpdateUserRoleResponse>(
                UserErrors.UserNotFound);
        }

        user.ChangeRole(request.Role);

        await _userRepository.UpdateAsync(user, cancellationToken);

        return Result.Success(
            new UpdateUserRoleResponse(
                user.Id.Value,
                user.Role.ToString()),
            "User role updated successfully.");
    }
}
