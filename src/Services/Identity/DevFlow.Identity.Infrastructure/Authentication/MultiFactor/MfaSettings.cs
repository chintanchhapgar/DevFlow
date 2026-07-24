using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Authentication.MultiFactor
{
    public sealed class MfaSettings
    {
        public const string SectionName = "Mfa";

        public string Issuer { get; init; } = "DevFlow";

        public int SecretLength { get; init; } = 20;

        public int RecoveryCodeCount { get; init; } = 10;
    }
}
