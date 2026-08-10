using System.Globalization;
using DevFlow.Notification.Infrastructure.Email.Configuration;
using DevFlow.Notification.Infrastructure.Email.Models;
using DevFlow.Notification.Infrastructure.Email.Rendering;
using DevFlow.Notification.Infrastructure.Email.Sending;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevFlow.Notification.Infrastructure.Email.Verification;

public sealed class VerificationEmailSender
{
    private const string TemplateName = "VerifyEmail.html";

    private readonly IEmailTemplateRenderer _templateRenderer;
    private readonly IEmailSender _emailSender;
    private readonly EmailSettings _settings;
    private readonly ILogger<VerificationEmailSender> _logger;

    public VerificationEmailSender(
        IEmailTemplateRenderer templateRenderer,
        IEmailSender emailSender,
        IOptions<EmailSettings> settings,
        ILogger<VerificationEmailSender> logger)
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
        Guid verificationToken,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);

        if (verificationToken == Guid.Empty)
        {
            throw new ArgumentException(
                "Verification token cannot be empty.",
                nameof(verificationToken));
        }

        if (string.IsNullOrWhiteSpace(
                _settings.VerificationBaseUrl))
        {
            throw new InvalidOperationException(
                "Email verification base URL is not configured.");
        }

        _logger.SendingVerificationEmail(email);

        var token = verificationToken.ToString(
            "D",
            CultureInfo.InvariantCulture);

        var verificationUrl =
            $"{_settings.VerificationBaseUrl.TrimEnd('/')}" +
            $"/verify-email?token={Uri.EscapeDataString(token)}";

        var html = await _templateRenderer.RenderAsync(
            TemplateName,
            new Dictionary<string, string>(
                StringComparer.Ordinal)
            {
                ["FirstName"] = firstName,
                ["VerificationUrl"] = verificationUrl,
                ["Year"] = DateTime.UtcNow.Year.ToString(
                    CultureInfo.InvariantCulture)
            },
            cancellationToken);

        var message = new EmailMessage
        {
            To = new EmailAddress(
                firstName,
                email),

            Subject = "Verify your DevFlow email",

            HtmlBody = html
        };

        await _emailSender.SendAsync(
            message,
            cancellationToken);

        _logger.VerificationEmailSent(email);
    }
}
