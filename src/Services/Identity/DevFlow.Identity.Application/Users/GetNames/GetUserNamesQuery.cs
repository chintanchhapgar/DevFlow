using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.GetNames;

public sealed record GetUserNamesQuery(
    IReadOnlyCollection<Guid> UserIds)
    : IRequest<Result<IReadOnlyList<UserNameResponse>>>;
