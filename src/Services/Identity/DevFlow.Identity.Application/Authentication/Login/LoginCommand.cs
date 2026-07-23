using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Login request.
/// </summary>
public sealed record LoginCommand(
    string Email,
    string Password)
    : IRequest<Result<LoginResponse>>;
