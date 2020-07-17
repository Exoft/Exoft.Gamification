using Exoft.Gamification.Api.Data.Core.Entities;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class RoleDumbData
    {
        public static Role GetEntity(string role)
        {
            return new Role
            {
                Name = role
            };
        }
    }
}
