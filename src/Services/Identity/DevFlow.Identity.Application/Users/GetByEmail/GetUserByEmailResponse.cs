using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Identity.Application.Users.GetByEmail
{
    public sealed record GetUserByEmailResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName);
}
