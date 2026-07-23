using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ForgotPassword;

public sealed record ForgotPasswordCommand(
    string Email)
    : IRequest<Result<ForgotPasswordResponse>>;
