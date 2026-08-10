using DevFlow.BuildingBlocks.Messaging.Outbox;
using DevFlow.Notification.Application.Common.Abstractions.Persistence;
using DevFlow.Notification.Infrastructure.Email.Options;
using DevFlow.Notification.Infrastructure.Email.PasswordReset;
using DevFlow.Notification.Infrastructure.Email.Rendering;
using DevFlow.Notification.Infrastructure.Email.Sending;
using DevFlow.Notification.Infrastructure.Email.Verification;
using DevFlow.Notification.Infrastructure.Persistence;
using DevFlow.Notification.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Notification.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddDbContext<NotificationDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("NotificationDatabase")));

        services.AddScoped<NotificationRepository,NotificationRepository>();

        services.AddTransactionalOutbox<NotificationDbContext>();

        services.AddScoped<INotificationRepository, NotificationRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<EmailOptions>(configuration.GetSection(EmailOptions.SectionName));

        services.AddScoped<IEmailSender, SmtpEmailSender>();

        services.AddScoped<IEmailTemplateRenderer,EmailTemplateRenderer>();

        services.AddScoped<IEmailSender,SmtpEmailSender>();

        services.AddScoped<VerificationEmailSender>();

        services.AddScoped<PasswordResetEmailSender>();

        return services;
    }
}
