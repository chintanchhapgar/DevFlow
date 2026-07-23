using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Register;

/// <summary>
/// Handles user registration.
/// </summary>
internal sealed class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailVerificationTokenGenerator _verificationTokenGenerator;
    private readonly IEmailVerificationTokenRepository _verificationRepository;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IEmailVerificationTokenGenerator verificationTokenGenerator,
        IEmailVerificationTokenRepository verificationRepository)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _verificationTokenGenerator = verificationTokenGenerator;
        _verificationRepository = verificationRepository;
    }

    public async Task<Result<RegisterResponse>> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByEmailAsync(
                request.Email,
                cancellationToken))
        {
            return Result.Failure<RegisterResponse>(
                UserErrors.EmailAlreadyExists);
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.Email,
            passwordHash,
            request.FirstName,
            request.LastName);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        var tokenValue =
             _verificationTokenGenerator.Generate();

        var verificationToken =
            EmailVerificationToken.Create(
                user.Id,
                tokenValue,
                DateTime.UtcNow.AddHours(24));

        await _verificationRepository.AddAsync(
            verificationToken,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);


        return new RegisterResponse(
            user.Id.Value,
            verificationToken.Token);
    }
}
