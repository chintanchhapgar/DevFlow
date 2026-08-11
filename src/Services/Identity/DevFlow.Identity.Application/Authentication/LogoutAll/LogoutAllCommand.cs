using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.LogoutAll;

public sealed record LogoutAllCommand
    : IRequest<Result<LogoutAllResponse>>;
