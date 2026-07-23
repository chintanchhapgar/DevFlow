using System.Security.Cryptography;
using DevFlow.Identity.Application.Common.Abstractions.Authentication;

namespace DevFlow.Identity.Infrastructure.Authentication;

internal sealed class PasswordResetTokenGenerator
    : IPasswordResetTokenGenerator
{
    public string Generate()
    {
        return Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));
    }
}
