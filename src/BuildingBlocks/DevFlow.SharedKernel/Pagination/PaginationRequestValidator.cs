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
    }
}
