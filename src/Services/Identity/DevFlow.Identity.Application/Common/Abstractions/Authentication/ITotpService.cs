using DevFlow.SharedKernel.Results;

namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

/// <summary>
/// Provides Time-based One-Time Password (TOTP) functionality.
/// </summary>
public interface ITotpService
{
    string GenerateSecret();

    string GenerateQrCodeUri(
        string issuer,
        string email,
        string secret);

    string GenerateQrCodeImage(
        string issuer,
        string email,
        string secret);

    Result VerifyCode(
        string secret,
        string code);
}
