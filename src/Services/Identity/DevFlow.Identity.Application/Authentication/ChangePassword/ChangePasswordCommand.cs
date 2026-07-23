using DevFlow.SharedKernel.Results;
using MediatR;

namespace DevFlow.Identity.Application.Authentication.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword)
    : IRequest<Result<ChangePasswordResponse>>;
