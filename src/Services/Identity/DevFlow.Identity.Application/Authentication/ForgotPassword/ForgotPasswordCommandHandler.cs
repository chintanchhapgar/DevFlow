using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Notifications;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.PasswordResetTokens;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ForgotPassword;

internal sealed class ForgotPasswordCommandHandler
    : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordResetTokenRepository _passwordResetRepository;
    private readonly IPasswordResetTokenGenerator _tokenGenerator;
    private readonly IEmailSender _emailSender;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordResetTokenRepository passwordResetRepository,
        IPasswordResetTokenGenerator tokenGenerator,
        IEmailSender emailSender)
    {
        _userRepository = userRepository;
        _passwordResetRepository = passwordResetRepository;
        _tokenGenerator = tokenGenerator;
        _emailSender = emailSender;
    }

    public async Task<Result<ForgotPasswordResponse>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        // Prevent email enumeration
        if (user is null)
        {
            return new ForgotPasswordResponse();
        }

        var tokenValue = _tokenGenerator.Generate();

        var resetToken = PasswordResetToken.Create(
            user.Id,
            tokenValue,
            DateTime.UtcNow.AddHours(1));       

        await _passwordResetRepository.AddAsync(
            resetToken,
            cancellationToken);

        await _emailSender.SendPasswordResetEmailAsync(
           user.Email,
           tokenValue,
           cancellationToken);

        // Email sending will be implemented next.
        // For now, the token is persisted.

        return new ForgotPasswordResponse();
    }
}
