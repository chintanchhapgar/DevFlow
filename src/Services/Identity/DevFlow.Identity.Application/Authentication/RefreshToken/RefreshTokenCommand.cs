using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken)
    : IRequest<Result<RefreshTokenResponse>>;
