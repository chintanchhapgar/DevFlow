using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Attachments.GetAll;

public sealed record GetAllAttachmentsQuery(
    Guid WorkItemId)
    : IRequest<Result<IReadOnlyList<GetAllAttachmentsResponse>>>;
