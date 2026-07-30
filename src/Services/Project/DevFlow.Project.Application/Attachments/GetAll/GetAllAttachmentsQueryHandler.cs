using DevFlow.Project.Domain.Attachments.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.GetAll;

internal sealed class GetAllAttachmentsQueryHandler
    : IRequestHandler<
        GetAllAttachmentsQuery,
        Result<IReadOnlyList<GetAllAttachmentsResponse>>>
{
    private readonly IAttachmentRepository _attachmentRepository;

    public GetAllAttachmentsQueryHandler(
        IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllAttachmentsResponse>>> Handle(
        GetAllAttachmentsQuery request,
        CancellationToken cancellationToken)
    {
        var attachments =
            await _attachmentRepository.GetByWorkItemAsync(
                request.WorkItemId,
                cancellationToken);

        var response = attachments
            .Select(x => new GetAllAttachmentsResponse(
                x.Id.Value,
                x.OriginalFileName,
                x.ContentType,
                x.Extension,
                x.SizeInBytes,
                x.CreatedOnUtc,
                x.UploadedBy))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyList<GetAllAttachmentsResponse>>(
            response,
            "Attachments retrieved successfully.");
    }
}
