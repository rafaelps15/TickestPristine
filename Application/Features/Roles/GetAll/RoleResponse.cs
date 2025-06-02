using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickest.Application.Features.Roles.GetAll
{
   public sealed record RoleResponse(Guid Id, string Name);
}
