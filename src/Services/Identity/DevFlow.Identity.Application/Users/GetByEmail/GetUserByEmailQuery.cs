using DevFlow.SharedKernel.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Identity.Application.Users.GetByEmail
{
    public sealed record GetUserByEmailQuery(
    string Email)
    : IRequest<Result<GetUserByEmailResponse?>>;
}
