namespace DevFlow.Identity.Application.Authentication.MultiFactor.Verify;

/// <summary>
/// Response returned after successfully enabling MFA.
/// Recovery codes are shown only once.
/// </summary>
public sealed record VerifyTwoFactorResponse(
    IReadOnlyList<string> RecoveryCodes);
