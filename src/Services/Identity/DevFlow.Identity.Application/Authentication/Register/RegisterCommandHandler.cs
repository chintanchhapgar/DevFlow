using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Register;

/// <summary>
/// Handles user registration.
/// Validates email uniqueness, hashes password, creates User aggregate.
/// </summary>
internal sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IIdentityUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        // Check email uniqueness
        var emailExists = await _userRepository.ExistsByEmailAsync(
            request.Email,
            cancellationToken);

        if (emailExists)
            return UserErrors.EmailAlreadyExists;

        // Hash password
        var passwordHash = _passwordHasher.Hash(request.Password);

        // Create User aggregate (domain validation)
        var userResult = User.Create(
            email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName,
            passwordHash: passwordHash);

        if (userResult.IsFailure)
            return userResult.Error;

        var user = userResult.Value;

        // Persist
        _userRepository.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RegisterResponse(
            UserId: user.Id.Value,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName);
    }
}
