using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.RefreshToken;

/// <summary>
/// Command to exchange a refresh token for a new access token.
/// </summary>
public sealed record RefreshTokenCommand(
    Guid UserId,
    string RefreshToken) : IRequest<Result<RefreshTokenResponse>>;
