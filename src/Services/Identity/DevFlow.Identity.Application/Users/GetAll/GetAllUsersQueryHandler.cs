using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.GetAll;

internal sealed class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQuery, Result<GetAllUsersResponse>>
{
    private readonly IUserRepository _repository;

    public GetAllUsersQueryHandler(
        IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetAllUsersResponse>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var (users, totalCount) = await _repository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.Search,
            cancellationToken);

        var response = new GetAllUsersResponse(
            users.Select(x => new UserItemResponse(
                x.Id.Value,
                x.Email,
                x.FirstName,
                x.LastName,
                x.FullName,
                x.Role.ToString()))
            .ToList(),
            request.Page,
            request.PageSize,
            totalCount);

        return Result.Success(response);
    }
}
