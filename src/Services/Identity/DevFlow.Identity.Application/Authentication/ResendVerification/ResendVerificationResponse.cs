using DevFlow.BuildingBlocks.Api.Responses;

namespace DevFlow.Identity.Application.Authentication.ResendVerification;

/// <summary>
/// Response returned after requesting a new verification email.
/// </summary>
public sealed record ResendVerificationResponse(
    string VerificationToken,
    string SuccessMessage)
    : IApiMessage
{
    public string Message => SuccessMessage;
}
