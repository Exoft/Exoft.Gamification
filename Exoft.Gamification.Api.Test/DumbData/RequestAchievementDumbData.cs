using Exoft.Gamification.Api.Common.Models;
using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class RequestAchievementDumbData
    {
        public static RequestAchievement GetRandomEntity()
        {
            return new RequestAchievement
            {
                AchievementId = new Guid(),
                Message = RandomHelper.GetRandomString(100),
                UserId = new Guid()
            };
        }

        public static List<RequestAchievement> GetRandomEntities(int number)
        {
            var list = new List<RequestAchievement>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static RequestAchievementModel GetRequestAchievementModel(Guid achievementId = new Guid())
        {
            return new RequestAchievementModel
            {
                AchievementId = achievementId,
                Message = RandomHelper.GetRandomString(100)
            };
        }

        public static ReadRequestAchievementModel GetRandomReadRequestAchievementModel()
        {
            return new ReadRequestAchievementModel
            {
                Id = new Guid(),
                Message = RandomHelper.GetRandomString(100),
                AchievementName = RandomHelper.GetRandomString(100),
                UserName = RandomHelper.GetRandomString(100),
                AchievementId = new Guid(),
                UserId = new Guid()
            };
        }

        public static List<ReadRequestAchievementModel> GetRandomReadRequestAchievementModels(int number)
        {
            var list = new List<ReadRequestAchievementModel>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomReadRequestAchievementModel());
            }
            return list;
        }
    }
}
