using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ResetPassword;

public sealed record ResetPasswordCommand(
    string Token,
    string NewPassword)
    : IRequest<Result<ResetPasswordResponse>>;
