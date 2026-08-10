using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace DevFlow.Notification.Infrastructure.Email.Rendering;

/// <summary>
/// Loads HTML email templates and replaces placeholders.
/// </summary>
internal sealed class EmailTemplateRenderer
    : IEmailTemplateRenderer
{
    private readonly IWebHostEnvironment _environment;

    public EmailTemplateRenderer(
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(environment);

        _environment = environment;
    }

    public async Task<string> RenderAsync(
        string templateName,
        IReadOnlyDictionary<string, string> values,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateName);
        ArgumentNullException.ThrowIfNull(values);

        var templatePath = Path.Combine(
            _environment.ContentRootPath,
            "Email",
            "Templates",
            templateName);

        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException(
                $"Email template '{templateName}' was not found.",
                templatePath);
        }

        var template = await File.ReadAllTextAsync(
            templatePath,
            Encoding.UTF8,
            cancellationToken);

        foreach (var pair in values)
        {
            var placeholder = $"{{{{{pair.Key}}}}}";

            template = template.Replace(
                placeholder,
                pair.Value,
                StringComparison.Ordinal);
        }

        return template;
    }
}
