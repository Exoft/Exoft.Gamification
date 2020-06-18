using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class UserDumbData
    {

        #region Entity
        public static User GetRandomEntity()
        {
            return new User
            {
                AvatarId = Guid.NewGuid(),
                FirstName = RandomHelper.GetRandomString(10),
                LastName = RandomHelper.GetRandomString(20),
                Email = RandomHelper.GetRandomEmail(10),
                Password = RandomHelper.GetRandomString(30),
                Roles = new List<UserRoles> { new UserRoles { Role = new Role { Name = GamificationRole.User } } },
                Status = RandomHelper.GetRandomString(10),
                UserName = RandomHelper.GetRandomString(10),
                XP = RandomHelper.GetRandomNumber()
            };
        }

        public static List<User> GetRandomEntities(int number)
        {
            var list = new List<User>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static User GetEntity(UpdateUserModel updateModel)
        {
            return new User
            {
                AvatarId = Guid.NewGuid(),
                FirstName = updateModel.FirstName,
                LastName = updateModel.LastName,
                Email = updateModel.Email,
                Password = RandomHelper.GetRandomString(30),
                Roles = new List<UserRoles> { new UserRoles { Role = new Role { Name = GamificationRole.User } } },
                Status = updateModel.Status,
                UserName = updateModel.UserName,
                XP = RandomHelper.GetRandomNumber()
            };
        }

        #endregion

        #region Models

        public static UpdateFullUserModel GetRandomUpdateFullUserModel()
        {
            return new UpdateFullUserModel
            {
                Avatar = null,
                FirstName = RandomHelper.GetRandomString(10),
                LastName = RandomHelper.GetRandomString(20),
                Email = RandomHelper.GetRandomEmail(10),
                Roles = new List<string> { GamificationRole.User },
                Status = RandomHelper.GetRandomString(10),
                UserName = RandomHelper.GetRandomString(10)
            };
        }

        public static CreateUserModel GetRandomCreateUserModel()
        {
            return new CreateUserModel
            {
                Avatar = null,
                Password = RandomHelper.GetRandomString(15),
                FirstName = RandomHelper.GetRandomString(10),
                LastName = RandomHelper.GetRandomString(20),
                Email = RandomHelper.GetRandomEmail(10),
                Roles = new List<string> { GamificationRole.User },
                Status = RandomHelper.GetRandomString(10),
                UserName = RandomHelper.GetRandomString(10)
            };
        }

        public static ReadFullUserModel GetReadFullUserModel(CreateUserModel user)
        {
            return new ReadFullUserModel
            {
                Id = Guid.NewGuid(),
                AvatarId = Guid.NewGuid(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.Roles,
                Status = user.Status,
                UserName = user.UserName
            };
        }

        #endregion

    }
}
