using DevFlow.Notification.Infrastructure.Email.Models;
using DevFlow.Notification.Infrastructure.Email.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DevFlow.Notification.Infrastructure.Email.Sending;

/// <summary>
/// Sends emails using SMTP.
/// </summary>
internal sealed class SmtpEmailSender : IEmailSender
{
    private readonly EmailOptions _options;

    public SmtpEmailSender(
        IOptions<EmailOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
    }

    public async Task SendAsync(
        EmailMessage message,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);

        ValidateConfiguration();

        var email = new MimeMessage();

        email.From.Add(
            new MailboxAddress(
                _options.SenderName,
                _options.SenderEmail));

        email.To.Add(
            new MailboxAddress(
                message.To.Name,
                message.To.Address));

        email.Subject = message.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.HtmlBody
        };

        email.Body = bodyBuilder.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(
            _options.Host,
            _options.Port,
            GetSecureSocketOptions(),
            cancellationToken);

        await client.AuthenticateAsync(
            _options.Username,
            _options.Password,
            cancellationToken);

        await client.SendAsync(
            email,
            cancellationToken);

        await client.DisconnectAsync(
            true,
            cancellationToken);
    }

    private SecureSocketOptions GetSecureSocketOptions()
    {
        return _options.Port switch
        {
            // Gmail SMTP submission
            587 => SecureSocketOptions.StartTls,

            // Gmail implicit TLS
            465 => SecureSocketOptions.SslOnConnect,

            // Other SMTP servers
            _ when _options.UseSsl =>
                SecureSocketOptions.StartTls,

            _ => SecureSocketOptions.None
        };
    }

    private void ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(_options.Host))
        {
            throw new InvalidOperationException(
                "Email SMTP host is not configured.");
        }

        if (_options.Port <= 0)
        {
            throw new InvalidOperationException(
                "Email SMTP port is not configured.");
        }

        if (string.IsNullOrWhiteSpace(_options.Username))
        {
            throw new InvalidOperationException(
                "Email SMTP username is not configured.");
        }

        if (string.IsNullOrWhiteSpace(_options.Password))
        {
            throw new InvalidOperationException(
                "Email SMTP password is not configured.");
        }

        if (string.IsNullOrWhiteSpace(_options.SenderEmail))
        {
            throw new InvalidOperationException(
                "Email sender address is not configured.");
        }
    }
}
