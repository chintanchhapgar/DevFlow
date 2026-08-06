using DevFlow.BuildingBlocks.Infrastructure.Outbox;
using DevFlow.BuildingBlocks.Infrastructure.Persistence.Interceptors;
using DevFlow.BuildingBlocks.Messaging;
using DevFlow.BuildingBlocks.Messaging.Outbox;
using DevFlow.Identity.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Identity;
using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Common.Abstractions.Storage;
using DevFlow.Project.Domain.Attachments.Repositories;
using DevFlow.Project.Domain.Comments.Repositories;
using DevFlow.Project.Domain.Epics.Repositories;
using DevFlow.Project.Domain.Labels.Repositories;
using DevFlow.Project.Domain.Sprints.Repositories;
using DevFlow.Project.Domain.WorkItems.Repositories;
using DevFlow.Project.Domain.Worklogs.Repositories;
using DevFlow.Project.Infrastructure.Identity;
using DevFlow.Project.Infrastructure.Outbox;
using DevFlow.Project.Infrastructure.Persistence;
using DevFlow.Project.Infrastructure.Persistence.Repositories;
using DevFlow.Project.Infrastructure.Persistence.Sorting;
using DevFlow.Project.Infrastructure.Seed.Projects;
using DevFlow.Project.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Project.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ProjectDbContext>((serviceProvider, options) =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("ProjectDb"));

            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditableInterceptor>(),
                serviceProvider.GetRequiredService<SoftDeleteInterceptor>());
        });

        services.AddScoped<IProjectRepository, ProjectRepository>();

        services.AddScoped<IWorkItemRepository, WorkItemRepository>();

        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ISprintRepository, SprintRepository>();

        services.AddScoped<ICommentRepository, CommentRepository>();

        services.AddScoped<IFileStorage, LocalFileStorage>();

        services.AddScoped<ILabelRepository, LabelRepository>();

        services.AddScoped<IEpicRepository, EpicRepository>();

        services.AddScoped<IWorklogRepository, WorklogRepository>();

        services.AddScoped<ProjectSorting>();
        services.AddScoped<SprintSorting>();
        services.AddScoped<WorkItemSorting>();
        services.AddScoped<ProjectSeeder>();

        services.AddScoped<IProjectReportRepository, ProjectReportRepository>();

        services.AddScoped<IBurndownRepository, BurndownRepository>();

        services.AddScoped<IVelocityRepository, VelocityRepository>();

        services.AddScoped<IWorkloadRepository, WorkloadRepository>();

        services.AddTransactionalOutbox<ProjectDbContext>();

        services.AddHostedService<ProjectOutboxProcessor>();

        services.AddHttpClient<IUserLookupService, UserLookupService>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["Services:Identity"]
                ?? throw new InvalidOperationException(
                    "Identity Service URL is missing."));
        });

        return services;
    }
}
