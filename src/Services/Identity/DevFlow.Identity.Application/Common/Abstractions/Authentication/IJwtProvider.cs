using DevFlow.Identity.Domain.Authentication;

namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
}
