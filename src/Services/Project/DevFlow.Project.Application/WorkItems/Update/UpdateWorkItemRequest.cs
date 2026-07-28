using System.ComponentModel.DataAnnotations;

namespace DevFlow.Project.Application.WorkItems.Update;

public sealed record UpdateWorkItemRequest(
    [Required]
    string Title,

    string? Description,

    DateTime? DueDate,

    decimal? EstimateHours);
