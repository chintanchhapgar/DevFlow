using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.Logout;

public sealed record LogoutCommand(
    string RefreshToken)
    : IRequest<Result<LogoutResponse>>;
