using DevFlow.BuildingBlocks.Contracts.IntegrationEvents.Identity;
using DevFlow.BuildingBlocks.Messaging.EventBus;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;
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
    private readonly IEventBus _eventBus;

    public ForgotPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordResetTokenRepository passwordResetRepository,
        IPasswordResetTokenGenerator tokenGenerator,
        IEventBus eventBus)
    {
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(passwordResetRepository);
        ArgumentNullException.ThrowIfNull(tokenGenerator);
        ArgumentNullException.ThrowIfNull(eventBus);

        _userRepository = userRepository;
        _passwordResetRepository = passwordResetRepository;
        _tokenGenerator = tokenGenerator;
        _eventBus = eventBus;
    }

    public async Task<Result<ForgotPasswordResponse>> Handle(
        ForgotPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        // Prevent email enumeration.
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

        var integrationEvent =
            new UserPasswordResetRequestedIntegrationEvent(
                user.Id.Value,
                user.Email,
                user.FirstName,
                tokenValue);

        await _eventBus.PublishAsync(
            integrationEvent,
            cancellationToken);

        return new ForgotPasswordResponse();
    }
}
