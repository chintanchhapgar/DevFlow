using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Register;

/// <summary>
/// Register a new user.
/// </summary>
public sealed record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName)
    : IRequest<Result<RegisterResponse>>;
