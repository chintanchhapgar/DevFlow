using System.Globalization;
using DevFlow.Notification.Infrastructure.Email.Configuration;
using DevFlow.Notification.Infrastructure.Email.Models;
using DevFlow.Notification.Infrastructure.Email.Rendering;
using DevFlow.Notification.Infrastructure.Email.Sending;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevFlow.Notification.Infrastructure.Email.PasswordReset;

public sealed class PasswordResetEmailSender
{
    private const string TemplateName = "ResetPassword.html";

    private readonly IEmailTemplateRenderer _templateRenderer;
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _settings;
    private readonly ILogger<PasswordResetEmailSender> _logger;

    public PasswordResetEmailSender(
        IEmailTemplateRenderer templateRenderer,
        IEmailSender emailSender,
        IOptions<EmailSettings> settings,
        ILogger<PasswordResetEmailSender> logger)
    {
        ArgumentNullException.ThrowIfNull(templateRenderer);
        ArgumentNullException.ThrowIfNull(emailSender);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(logger);

        _templateRenderer = templateRenderer;
        _emailSender = emailSender;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendAsync(
        string email,
        string firstName,
        string resetToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(resetToken);

        if (string.IsNullOrWhiteSpace(
                _settings.FrontendBaseUrl))
        {
            throw new InvalidOperationException(
                "Email verification base URL is not configured.");
        }

        var resetUrl =
            $"{_settings.FrontendBaseUrl.TrimEnd('/')}" +
            $"/reset-password?token={Uri.EscapeDataString(resetToken)}";

        var html = await _templateRenderer.RenderAsync(
            TemplateName,
            new Dictionary<string, string>(
                StringComparer.Ordinal)
            {
                ["FirstName"] = firstName,
                ["ResetUrl"] = resetUrl,
                ["Year"] = DateTime.UtcNow.Year.ToString(
                    CultureInfo.InvariantCulture)
            },
            cancellationToken);

        var message = new EmailMessage
        {
            To = new EmailAddress(
                firstName,
                email),

            Subject = "Reset your DevFlow password",

            HtmlBody = html
        };

        await _emailSender.SendAsync(
            message,
            cancellationToken);

        _logger.PasswordResetEmailSent(email);
    }
}
