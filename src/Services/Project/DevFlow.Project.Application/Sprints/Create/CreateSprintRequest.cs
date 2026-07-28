using System.ComponentModel.DataAnnotations;

namespace DevFlow.Project.Application.Sprints.Create;

public sealed record CreateSprintRequest(
    [Required]
    string Name,

    string? Goal,

    DateOnly StartDate,

    DateOnly EndDate);
