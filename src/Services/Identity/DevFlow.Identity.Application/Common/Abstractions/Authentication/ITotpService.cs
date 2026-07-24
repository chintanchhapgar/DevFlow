using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Provides Time-based One-Time Password (TOTP) functionality.
/// </summary>
public interface ITotpService
{
    /// <summary>
    /// Generates a new Base32 encoded secret.
    /// </summary>
    string GenerateSecret();

    /// <summary>
    /// Builds the otpauth:// URI used by authenticator apps.
    /// </summary>
    string GenerateQrCodeUri(
        string issuer,
        string email,
        string secret);

    /// <summary>
    /// Verifies a 6-digit TOTP code.
    /// </summary>
    Result VerifyCode(
        string secret,
        string code);
}
