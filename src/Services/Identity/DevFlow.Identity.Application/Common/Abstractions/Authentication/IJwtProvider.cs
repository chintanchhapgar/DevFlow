using DevFlow.Identity.Domain.Authentication.Users;

namespace DevFlow.Identity.Application.Common.Abstractions.Authentication;

public interface IJwtProvider
{
    string GenerateAccessToken(User user);
}
