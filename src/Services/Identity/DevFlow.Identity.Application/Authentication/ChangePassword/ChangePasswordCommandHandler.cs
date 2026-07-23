using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ChangePassword;

internal sealed class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public ChangePasswordCommandHandler(
        IUserRepository userRepository,
        ICurrentUser currentUser,
        IPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _currentUser = currentUser;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<Result<ChangePasswordResponse>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            _currentUser.UserId,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<ChangePasswordResponse>(
                UserErrors.UserNotFound);
        }

        if (!_passwordHasher.Verify(
                request.CurrentPassword,
                user.PasswordHash))
        {
            return Result.Failure<ChangePasswordResponse>(
                UserErrors.InvalidCurrentPassword);
        }

        user.ChangePassword(
            _passwordHasher.Hash(request.NewPassword));

        var refreshTokens =
            await _refreshTokenRepository.GetActiveByUserIdAsync(
                user.Id,
                cancellationToken);

        foreach (var token in refreshTokens)
        {
            token.Revoke();
            await _refreshTokenRepository.UpdateAsync(
                token,
                cancellationToken);
        }

        return new ChangePasswordResponse(true);
    }
}
