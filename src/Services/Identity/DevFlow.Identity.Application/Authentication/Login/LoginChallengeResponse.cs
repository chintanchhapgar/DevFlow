using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.Login;

/// <summary>
/// Returned when the user's password is correct,
/// but two-factor authentication must be completed.
/// </summary>
public sealed record LoginChallengeResponse(
    Guid UserId,
    bool RequiresTwoFactor)
    : IApiMessage
{
    public string Message => "Two-factor authentication required.";
}
