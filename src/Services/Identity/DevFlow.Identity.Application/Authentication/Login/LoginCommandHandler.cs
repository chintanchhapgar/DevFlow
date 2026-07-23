using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Domain.Authentication;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Handles user login.
/// </summary>
internal sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(
                UserErrors.InvalidCredentials);
        }

        if (!_passwordHasher.Verify(
                request.Password,
                user.PasswordHash))
        {
            return Result.Failure<LoginResponse>(
                UserErrors.InvalidCredentials);
        }

        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>(
                UserErrors.UserInactive);
        }

        var accessToken = _jwtProvider.GenerateAccessToken(user.Id.Value);

        return Result.Success(
            new LoginResponse(
                user.Id.Value,
                accessToken));
    }
}
