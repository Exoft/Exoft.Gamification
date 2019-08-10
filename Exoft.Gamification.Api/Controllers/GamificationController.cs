using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Exoft.Gamification.Api.Controllers
{
    public abstract class GamificationController : ControllerBase
    {
        protected Guid UserId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        protected string UserRole => User.FindFirst(ClaimTypes.Role)?.Value;
    }
}