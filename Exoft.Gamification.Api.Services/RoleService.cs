using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Services.Interfaces.Services;
using System.Security.Claims;
using static Exoft.Gamification.Api.Data.Core.Helpers.GamificationEnums;

namespace Exoft.Gamification.Api.Services
{
    public class RoleService : IRoleService
    {
        public string GetStringUserRole(RoleType userRole)
        {
            switch (userRole)
            {
                case RoleType.None:
                    return string.Empty;
                case RoleType.User:
                    return GamificationRole.User;
                case RoleType.Admin:
                    return GamificationRole.Admin;
                case RoleType.SuperAdmin:
                    return GamificationRole.SuperAdmin;
                default:
                    return string.Empty;
            }
        }

        public RoleType GetUserRoleTypeFromString(string roleStr)
        {
            if (string.IsNullOrEmpty(roleStr)) return RoleType.None;
            switch (roleStr)
            {
                case GamificationRole.User: return RoleType.User;
                case GamificationRole.Admin: return RoleType.Admin;
                case GamificationRole.SuperAdmin: return RoleType.SuperAdmin;
                default:
                    return RoleType.None;
            }
        }

        public bool CalculateAllowOperationsByUsersRole(RoleType userRoleWhoDoOperation, RoleType anotherUser)
        {
            return (int)userRoleWhoDoOperation - (int)anotherUser > 0;
        }

        public bool CalculateAllowOperationsByUsersRole(ClaimsPrincipal userClaimsWhoDoOperation, RoleType roleAnotherUser)
        {
            var roleWhoDoOperation = GetUserRoleTypeFromClaims(userClaimsWhoDoOperation);
            return CalculateAllowOperationsByUsersRole(roleWhoDoOperation, roleAnotherUser);
        }

        public bool CalculateAllowOperationsByUsersRole(ClaimsPrincipal userClaimsWhoDoOperation, string roleAnotherUserString)
        {
            var userRoleWhoDoOperation = GetUserRoleTypeFromClaims(userClaimsWhoDoOperation);
            var roleAnotherUser = GetUserRoleTypeFromString(roleAnotherUserString);
            return CalculateAllowOperationsByUsersRole(userRoleWhoDoOperation, roleAnotherUser);
        }

        private RoleType GetRole(object roleObj)
        {
            var roleString = roleObj.ToString();
            if (string.IsNullOrEmpty(roleString)) return RoleType.None;
            switch (roleString)
            {
                case GamificationRole.User: return RoleType.User;
                case GamificationRole.Admin: return RoleType.Admin;
                case GamificationRole.SuperAdmin: return RoleType.SuperAdmin;
                default:
                    return RoleType.None;
            }
        }

        public RoleType GetUserRoleTypeFromClaims(ClaimsPrincipal claims)
        {
            var claim = claims.FindFirst(ClaimTypes.Role);
            if (claim == null) return RoleType.None;
            var roleString = claim.Value;
            return GetRole(roleString);
        }
    }
}
