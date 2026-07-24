using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.SharedKernel.Results;
using OtpNet;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevFlow.Identity.Infrastructure.Authentication.MultiFactor;

/// <summary>
/// Default TOTP implementation.
/// </summary>
internal sealed class TotpService : ITotpService
{
    private const string Issuer = "DevFlow";

    public string GenerateSecret()
    {
        byte[] secret = RandomNumberGenerator.GetBytes(20);

        return Base32Encoding.ToString(secret);
    }

    public string GenerateQrCodeUri(
     string issuer,
     string email,
     string secret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(email);
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        return $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(email)}" +
               $"?secret={secret}" +
               $"&issuer={Uri.EscapeDataString(issuer)}" +
               $"&algorithm=SHA1" +
               $"&digits=6" +
               $"&period=30";
    }

    public Result VerifyCode(
        string secret,
        string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        byte[] secretBytes =
            Base32Encoding.ToBytes(secret);

        var totp = new Totp(secretBytes);

        bool valid = totp.VerifyTotp(
            code.Trim(),
            out _,
            new VerificationWindow(
                previous: 1,
                future: 1));

        if (!valid)
        {
            return Result.Failure(
                AppError.Validation(
                    "Mfa.InvalidCode",
                    "The verification code is invalid."));
        }

        return Result.Success();
    }
}
