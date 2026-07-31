using DevFlow.Project.Application.Common.Abstractions.Persistence;
using DevFlow.Project.Application.Projects.Create;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DevFlow.Project.Infrastructure.Seed.Projects;

internal sealed class ProjectSeeder
{
    private readonly IProjectRepository _repository;
    private readonly ISender _sender;
    private readonly ILogger<ProjectSeeder> _logger;

    public ProjectSeeder(
        IProjectRepository repository,
        ISender sender,
        ILogger<ProjectSeeder> logger)
    {
        _repository = repository;
        _sender = sender;
        _logger = logger;
    }

    public async Task SeedAsync(
        CancellationToken cancellationToken = default)
    {
        foreach (var project in DemoProjects.All)
        {
            await SeedProjectAsync(
                project,
                cancellationToken);
        }
    }

    private async Task SeedProjectAsync(
        DemoProject project,
        CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByKeyAsync(
                project.Key,
                cancellationToken))
        {
            //_logger.LogInformation(
            //    "Project {Key} already exists.",
            //    project.Key);

            return;
        }

        var result =
            await _sender.Send(
                new CreateProjectCommand(
                    project.Key,
                    project.Name,
                    project.Description,
                    project.Visibility),
                cancellationToken);

        if (result.IsFailure)
        {
            //_logger.LogWarning(
            //    "Failed to create project {Key}. Reason: {Reason}",
            //    project.Key,
            //    result.Error.Description);

            return;
        }

        //_logger.LogInformation(
        //    "Created project {Key}.",
        //    project.Key);
    }
}
