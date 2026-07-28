using System.ComponentModel.DataAnnotations;

namespace DevFlow.Project.Application.Sprints.Update;

public sealed record UpdateSprintRequest(
    [Required]
    string Name,

    string? Goal,

    DateOnly StartDate,

    DateOnly EndDate);
