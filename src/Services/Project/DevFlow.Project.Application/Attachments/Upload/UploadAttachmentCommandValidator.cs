using FluentValidation;

namespace DevFlow.Project.Application.Attachments.Upload;

internal sealed class UploadAttachmentCommandValidator
    : AbstractValidator<UploadAttachmentCommand>
{
    private static readonly string[] AllowedExtensions =
    {
        ".pdf",
        ".png",
        ".jpg",
        ".jpeg",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx",
        ".zip",
        ".txt"
    };

    public UploadAttachmentCommandValidator()
    {
        RuleFor(x => x.WorkItemId)
            .NotEmpty();

        RuleFor(x => x.File)
            .NotNull();

        RuleFor(x => x.File.Length)
            .LessThanOrEqualTo(25 * 1024 * 1024)
            .WithMessage("Maximum file size is 25 MB.");

        RuleFor(x => x.File.FileName)
            .Must(name =>
                AllowedExtensions.Contains(
                    Path.GetExtension(name).ToLowerInvariant()))
            .WithMessage("Unsupported file type.");
    }
}
