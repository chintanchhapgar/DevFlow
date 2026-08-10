namespace DevFlow.Notification.Infrastructure.Email.Rendering;

/// <summary>
/// Renders an email template using supplied placeholder values.
/// </summary>
public interface IEmailTemplateRenderer
{
    Task<string> RenderAsync(
        string templateName,
        IReadOnlyDictionary<string, string> values,
        CancellationToken cancellationToken = default);
}
