using DevFlow.SharedKernel.Results;

namespace DevFlow.SharedKernel.Exceptions;

public class DomainException : Exception
{
    public DomainException(AppError appError)
        : base(appError.Description)
    {
        AppError = appError;
    }

    public AppError AppError { get; }
}
