using DevFlow.SharedKernel.Results;

namespace DevFlow.SharedKernel.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(AppError appError)
        : base(appError)
    {
    }
}
