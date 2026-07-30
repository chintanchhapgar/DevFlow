using FluentValidation;

namespace DevFlow.SharedKernel.Pagination;

public sealed class PaginationRequestValidator
    : AbstractValidator<PaginationRequest>
{
    public PaginationRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortDirection)
        .Must(x =>
            string.IsNullOrWhiteSpace(x) ||
            x.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
            x.Equals("desc", StringComparison.OrdinalIgnoreCase))
        .WithMessage("SortDirection must be 'asc' or 'desc'.");
    }
}
