using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Users.UpdateProfile;

/// <summary>
/// Command to update the authenticated user's profile.
/// </summary>
public sealed record UpdateProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName) : IRequest<Result>;
