using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ResendVerification;

/// <summary>
/// Handles resending email verification.
/// </summary>
internal sealed class ResendVerificationCommandHandler
    : IRequestHandler<ResendVerificationCommand, Result<ResendVerificationResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailVerificationTokenRepository _verificationRepository;
    private readonly IEmailVerificationTokenGenerator _tokenGenerator;

    public ResendVerificationCommandHandler(
        IUserRepository userRepository,
        IEmailVerificationTokenRepository verificationRepository,
        IEmailVerificationTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _verificationRepository = verificationRepository;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<ResendVerificationResponse>> Handle(
        ResendVerificationCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        // Prevent email enumeration.
        if (user is null)
        {
            return new ResendVerificationResponse(
                "If the account exists, a verification email has been sent.",
                 string.Empty);
        }

        if (user.EmailConfirmed)
        {
            return new ResendVerificationResponse(
                "Email is already verified.",
                 string.Empty);
        }

        var activeTokens =
            await _verificationRepository.GetActiveByUserIdAsync(
                user.Id,
                cancellationToken);

        foreach (var token in activeTokens)
        {
            token.Expire();

            await _verificationRepository.UpdateAsync(
                token,
                cancellationToken);
        }

        var tokenValue = _tokenGenerator.Generate();

        var verificationToken =
            EmailVerificationToken.Create(
                user.Id,
                tokenValue,
                DateTime.UtcNow.AddHours(24));

        await _verificationRepository.AddAsync(
            verificationToken,
            cancellationToken);

        // Notification event will be published here later.

        return new ResendVerificationResponse(
            "Verification email has been sent.",
            verificationToken.Token);
    }
}
