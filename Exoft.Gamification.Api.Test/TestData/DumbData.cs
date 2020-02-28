using Exoft.Gamification.Api.Common.Models.User;
using Exoft.Gamification.Api.Data.Core.Helpers;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test
{
    public static class DumbData
    {
        public static ReturnPagingInfo<T> GetReturnPagingInfoForModel<T>(PagingInfo pagingInfo, List<T> data)
        {
            var users = new ReturnPagingInfo<T>
            {
                Data = data,
                CurrentPage = pagingInfo.CurrentPage,
                PageSize = pagingInfo.PageSize,
                TotalItems = data.Count,
                TotalPages = (int)Math.Ceiling((double)data.Count / (pagingInfo.PageSize == 0 ? data.Count : pagingInfo.PageSize))
            };
            return users;
        }

        public static ReadShortUserModel GetRandomReadShortUserModel()
        {
            return new ReadShortUserModel
            {
                Id = Guid.NewGuid(),
                AvatarId = null,
                FirstName = RandomHelper.GetRandomString(10),
                LastName = RandomHelper.GetRandomString(20),
                XP = RandomHelper.GetRandomNumber()
            };
        }

        public static ReadFullUserModel GetRandomReadFullUserModel()
        {
            return new ReadFullUserModel
            {
                Id = Guid.NewGuid(),
                AvatarId = null,
                FirstName = RandomHelper.GetRandomString(10),
                LastName = RandomHelper.GetRandomString(20),
                BadgetCount = RandomHelper.GetRandomNumber(),
                Email = RandomHelper.GetRandomEmail(10),
                Roles = new List<string> { GamificationRole.User },
                Status = RandomHelper.GetRandomString(10),
                UserName = RandomHelper.GetRandomString(10),
                XP = RandomHelper.GetRandomNumber()
            };
        }

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

        public static List<ReadShortUserModel> GetRandomListReadShortUserModel(int number)
        {
            var list = new List<ReadShortUserModel>();
            for (int i = 0; i < number; i++)
            {
                list.Add(new ReadShortUserModel
                {
                    Id = Guid.NewGuid(),
                    AvatarId = null,
                    FirstName = RandomHelper.GetRandomString(10),
                    LastName = RandomHelper.GetRandomString(20),
                    XP = RandomHelper.GetRandomNumber()
                });
            }
            return list;
        }

        public static List<ReadFullUserModel> GetRandomListFullShortUserModel(int number)
        {
            var list = new List<ReadFullUserModel>();
            for (int i = 0; i < number; i++)
            {
                list.Add(new ReadFullUserModel
                {
                    Id = Guid.NewGuid(),
                    AvatarId = null,
                    FirstName = RandomHelper.GetRandomString(10),
                    LastName = RandomHelper.GetRandomString(20),
                    BadgetCount = RandomHelper.GetRandomNumber(),
                    Email = RandomHelper.GetRandomEmail(10),
                    Roles = new List<string> { GamificationRole.User },
                    Status = RandomHelper.GetRandomString(10),
                    UserName = RandomHelper.GetRandomString(10),
                    XP = RandomHelper.GetRandomNumber()
                });
            }
            return list;
        }

        public static ReadFullUserModel GetReadFullUserModelById(Guid userId)
        {
            return new ReadFullUserModel
            {
                Id = userId,
                AvatarId = null,
                FirstName = RandomHelper.GetRandomString(10),
                LastName = RandomHelper.GetRandomString(20),
                BadgetCount = RandomHelper.GetRandomNumber(),
                Email = RandomHelper.GetRandomEmail(10),
                Roles = new List<string> { GamificationRole.User },
                Status = RandomHelper.GetRandomString(10),
                UserName = RandomHelper.GetRandomString(10),
                XP = RandomHelper.GetRandomNumber()
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
                AvatarId = null,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.Roles,
                Status = user.Status,
                UserName = user.UserName
            };
        }

        public static ReadFullUserModel GetReadFullUserModel(UpdateFullUserModel user, Guid userId)
        {
            return new ReadFullUserModel
            {
                Id = userId,
                AvatarId = null,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.Roles,
                Status = user.Status,
                UserName = user.UserName
            };
        }
    }
}
