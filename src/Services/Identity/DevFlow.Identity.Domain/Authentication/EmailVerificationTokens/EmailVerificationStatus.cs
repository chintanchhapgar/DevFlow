namespace DevFlow.Identity.Domain.Authentication.EmailVerificationTokens;

/// <summary>
/// Status of an email verification token.
/// </summary>
public enum EmailVerificationStatus
{
    Active = 1,
    Used = 2,
    Expired = 3
}
