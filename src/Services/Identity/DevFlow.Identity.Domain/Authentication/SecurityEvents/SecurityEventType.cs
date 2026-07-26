namespace DevFlow.Identity.Domain.Authentication.SecurityEvents;

public enum SecurityEventType
{
    LoginSucceeded = 1,
    LoginFailed = 2,

    Logout = 3,

    RefreshTokenRotated = 4,

    SessionRevoked = 5,
    AllSessionsRevoked = 6,
    OtherSessionsRevoked = 7,

    TwoFactorEnabled = 8,
    TwoFactorDisabled = 9,
    RecoveryCodeUsed = 10,

    PasswordChanged = 11,
    PasswordReset = 12,

    EmailVerified = 13,

    AccountLocked = 14
}
