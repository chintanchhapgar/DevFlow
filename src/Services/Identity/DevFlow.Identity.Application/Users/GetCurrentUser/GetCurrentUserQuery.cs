using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.GetCurrentUser;

/// <summary>
/// Query to retrieve the authenticated user's profile.
/// </summary>
public sealed record GetCurrentUserQuery(Guid UserId)
    : IRequest<Result<CurrentUserResponse>>;
