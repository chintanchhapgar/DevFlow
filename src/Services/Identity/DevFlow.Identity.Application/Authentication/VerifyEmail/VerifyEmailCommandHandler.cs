using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Application.Common.Abstractions.Security;
using DevFlow.Identity.Domain.Authentication.SecurityEvents;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.VerifyEmail;

internal sealed class VerifyEmailCommandHandler
    : IRequestHandler<VerifyEmailCommand, Result<VerifyEmailResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ISecurityEventLogger _securityEventLogger;

    public VerifyEmailCommandHandler(
        IUserRepository userRepository,
        ISecurityEventLogger securityEventLogger)
    {
        _userRepository = userRepository;
        _securityEventLogger = securityEventLogger;
    }

    public async Task<Result<VerifyEmailResponse>> Handle(
        VerifyEmailCommand request,
        CancellationToken cancellationToken)
    {
        var user =
            await _userRepository.GetByEmailVerificationTokenAsync(
                request.Token,
                cancellationToken);

        if (user is null)
        {
            return Result.Failure<VerifyEmailResponse>(
                UserErrors.InvalidVerificationToken);
        }

        var result = user.VerifyEmail(request.Token);

        if (result.IsFailure)
        {
            return Result.Failure<VerifyEmailResponse>(
                result.Error);
        }

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        await _securityEventLogger.LogAsync(
            user.Id,
            SecurityEventType.EmailVerified,
            cancellationToken: cancellationToken);

        return new VerifyEmailResponse(
            user.Id.Value);
    }
}
