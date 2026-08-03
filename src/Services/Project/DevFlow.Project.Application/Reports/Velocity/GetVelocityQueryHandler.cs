using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Domain.Projects.Errors;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Project.Application.Reports.Velocity;

internal sealed class GetVelocityQueryHandler
    : IRequestHandler<
        GetVelocityQuery,
        Result<GetVelocityResponse>>
{
    private readonly IVelocityRepository _repository;

    public GetVelocityQueryHandler(
        IVelocityRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetVelocityResponse>> Handle(
        GetVelocityQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _repository.GetAsync(
            request.ProjectId,
            cancellationToken);

        if (response is null)
        {
            return Result.Failure<GetVelocityResponse>(
                ProjectErrors.NotFound);
        }

        return Result.Success(response);
    }
}
