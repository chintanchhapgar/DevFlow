using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ResendVerification;

internal sealed class ResendVerificationCommandHandler
    : IRequestHandler<
        ResendVerificationCommand,
        Result<ResendVerificationResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ResendVerificationCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
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
                "If the account exists, a verification email has been sent.");
        }

        if (user.EmailVerified)
        {
            return new ResendVerificationResponse(
                "Email is already verified.");
        }

        var tokenResult =
            user.GenerateNewEmailVerificationToken();

        if (tokenResult.IsFailure)
        {
            return Result.Failure<ResendVerificationResponse>(
                tokenResult.Error);
        }

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new ResendVerificationResponse(
            "Verification email has been sent.");
    }
}
