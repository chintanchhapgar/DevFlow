using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.GetAll;

public sealed record GetAllUsersQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null)
    : IRequest<Result<GetAllUsersResponse>>;
