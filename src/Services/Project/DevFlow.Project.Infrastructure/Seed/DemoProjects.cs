using DevFlow.Project.Domain.Projects.Enums;

namespace DevFlow.Project.Infrastructure.Seed;

internal static class DemoProjects
{
    public static IReadOnlyList<DemoProject> All =>
    [
        new(
            "DEV",
            "DevFlow",
            "Modern Jira-inspired project management platform.",
            ProjectVisibility.Private),

        new(
            "SHOP",
            "ShopSphere",
            "Enterprise e-commerce platform.",
            ProjectVisibility.Private),

        new(
            "LINK",
            "Linkly",
            "URL shortening and analytics platform.",
            ProjectVisibility.Public)
    ];
}

internal sealed record DemoProject(
    string Key,
    string Name,
    string? Description,
    ProjectVisibility Visibility);
