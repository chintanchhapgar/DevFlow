using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

/// <summary>
/// Handles email verification.
/// </summary>
internal sealed class VerifyEmailCommandHandler
    : IRequestHandler<VerifyEmailCommand, Result<VerifyEmailResponse>>
{
    private readonly IEmailVerificationTokenRepository _verificationRepository;
    private readonly IUserRepository _userRepository;

    public VerifyEmailCommandHandler(
        IEmailVerificationTokenRepository verificationRepository,
        IUserRepository userRepository)
    {
        _verificationRepository = verificationRepository;
        _userRepository = userRepository;
    }

    public async Task<Result<VerifyEmailResponse>> Handle(
        VerifyEmailCommand request,
        CancellationToken cancellationToken)
    {
        var verification =
            await _verificationRepository.GetByTokenAsync(
                request.Token,
                cancellationToken);

        if (verification is null || !verification.IsActive)
        {
            return Result.Failure<VerifyEmailResponse>(
                UserErrors.InvalidVerificationToken);
        }

        var user =
            await _userRepository.GetByIdAsync(
                verification.UserId,
                cancellationToken);

        if (user is null)
        {
            return Result.Failure<VerifyEmailResponse>(
                UserErrors.NotFound);
        }

        user.ConfirmEmail();

        verification.MarkAsUsed();

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _verificationRepository.UpdateAsync(
            verification,
            cancellationToken);

        return new VerifyEmailResponse(
            user.Id.Value,
            "Email verified successfully.");
    }
}
