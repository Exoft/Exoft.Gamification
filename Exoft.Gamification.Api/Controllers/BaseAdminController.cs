using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Controllers
{
    public abstract class BaseAdminController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        protected string UserRole => User.FindFirst(ClaimTypes.Role)?.Value;
    }
}
