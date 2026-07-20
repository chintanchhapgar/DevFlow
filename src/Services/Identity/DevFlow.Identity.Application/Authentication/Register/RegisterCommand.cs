using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Register;

/// <summary>
/// Command to register a new user account.
/// </summary>
public sealed record RegisterCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : IRequest<Result<RegisterResponse>>;
