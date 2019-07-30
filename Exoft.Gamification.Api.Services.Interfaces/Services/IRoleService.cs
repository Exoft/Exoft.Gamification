using System.Security.Claims;
using System.Collections.Generic;
using static Exoft.Gamification.Api.Data.Core.Helpers.GamificationEnums;

namespace Exoft.Gamification.Api.Services.Interfaces.Services
{
    public interface IRoleService
    {
        string GetStringUserRole(RoleType userRole);
        bool CalculateAllowOperationsByUsersRole(RoleType userRoleWhoDoOperation, RoleType anotherUser);
        bool CalculateAllowOperationsByUsersRole(ClaimsPrincipal userClaimsWhoDoOperation, RoleType roleAnotherUser);
        bool CalculateAllowOperationsByUsersRole(ClaimsPrincipal userClaimsWhoDoOperation, string roleAnotherUserString);
        bool CalculateAllowOperationsByUsersRole(ClaimsPrincipal userClaimsWhoDoOperation, IEnumerable<string> roles);
        RoleType GetUserRoleTypeFromString(string roleStr);
        RoleType GetUserRoleTypeFromClaims(ClaimsPrincipal claims);
        RoleType GetMaxUserRoleTypeFromRoles(IEnumerable<string> roles);
    }
}
