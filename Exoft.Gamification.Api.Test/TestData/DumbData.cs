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

        //public static List<T> GetList<T>(int number)
        //{
        //    var list = new List<T>();
        //    for (int i = 0; i < number; i++)
        //    {
        //        list.Add(GetItem<T>());
        //    }
        //    return list;
        //}

        //private static T GetItem<T>()
        //{
        //    switch (Type.GetTypeCode(typeof(T)))
        //    {
        //        case ReadShortUserModel:
        //            {
        //                return new ReadShortUserModel
        //                {
        //                    Id = Guid.NewGuid(),
        //                    AvatarId = null,
        //                    FirstName = RandomHelper.GetRandomString(10),
        //                    LastName = RandomHelper.GetRandomString(20),
        //                    XP = RandomHelper.GetRandomNumber()
        //                };
        //            }
        //        default: return default;
        //        case null:
        //            throw new ArgumentNullException(nameof(T));
        //    }
        //}

        public static List<ReadShortUserModel> GetListReadShortUserModel(int number)
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
    }
}
