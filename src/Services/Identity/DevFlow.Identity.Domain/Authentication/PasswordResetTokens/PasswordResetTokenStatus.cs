namespace DevFlow.Identity.Domain.Authentication.PasswordResetTokens;

public enum PasswordResetTokenStatus
{
    Active = 1,
    Used = 2,
    Expired = 3
}
