using DevFlow.Identity.Application.Common.Abstractions.Authentication;
using DevFlow.Identity.Application.Common.Abstractions.Notifications;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Identity.Domain.Authentication.Users;
using DevFlow.Identity.Infrastructure.Authentication;
using DevFlow.Identity.Infrastructure.Notifications;
using DevFlow.Identity.Infrastructure.Persistence;
using DevFlow.Identity.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Identity.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(
            configuration.GetSection(JwtSettings.SectionName));

        services.AddDbContext<IdentityDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("IdentityDatabase")));

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        services.AddScoped<IJwtProvider, JwtProvider>();

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();

        services.AddScoped<IPasswordResetTokenRepository,PasswordResetTokenRepository>();

        services.AddSingleton<IPasswordResetTokenGenerator,PasswordResetTokenGenerator>();

        services.AddScoped<IEmailSender, ConsoleEmailSender>();

        services.AddScoped<IEmailVerificationTokenRepository,EmailVerificationTokenRepository>();

        services.AddSingleton<IEmailVerificationTokenGenerator,EmailVerificationTokenGenerator>();

        return services;
    }
}
