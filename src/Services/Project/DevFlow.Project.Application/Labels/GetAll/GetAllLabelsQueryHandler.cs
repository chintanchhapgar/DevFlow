using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Labels.GetAll;

internal sealed class GetAllLabelsQueryHandler
    : IRequestHandler<
        GetAllLabelsQuery,
        Result<IReadOnlyList<GetAllLabelsResponse>>>
{
    private readonly ILabelRepository _labelRepository;

    public GetAllLabelsQueryHandler(
        ILabelRepository labelRepository)
    {
        _labelRepository = labelRepository;
    }

    public async Task<Result<IReadOnlyList<GetAllLabelsResponse>>> Handle(
        GetAllLabelsQuery request,
        CancellationToken cancellationToken)
    {
        var labels =
            await _labelRepository.GetByProjectAsync(
                request.ProjectId,
                cancellationToken);

        var response = labels
            .Select(x => new GetAllLabelsResponse(
                x.Id.Value,
                x.ProjectId,
                x.Name,
                x.Color))
            .ToList()
            .AsReadOnly();

        return Result.Success<IReadOnlyList<GetAllLabelsResponse>>(
            response,
            "Labels retrieved successfully.");
    }
}
