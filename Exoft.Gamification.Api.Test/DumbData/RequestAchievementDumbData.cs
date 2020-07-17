using Exoft.Gamification.Api.Common.Models.RequestAchievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class RequestAchievementDumbData
    {
        public static RequestAchievement GetRandomEntity()
        {
            return new RequestAchievement
            {
                AchievementId = Guid.NewGuid(),
                Message = RandomHelper.GetRandomString(100),
                UserId = Guid.NewGuid()
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

        public static RequestAchievement GetEntity(Achievement achievement, User user)
        {
            return new RequestAchievement
            {
                AchievementId = achievement.Id,
                Message = RandomHelper.GetRandomString(100),
                UserId = user.Id
            };
        }

        public static List<RequestAchievement> GetEntities(List<Achievement> achievements, List<User> users)
        {
            var list = new List<RequestAchievement>();
            var count = users.Count < achievements.Count ? users.Count : achievements.Count;
            for (int i = 0; i < count; i++)
            {
                list.Add(GetEntity(achievements[i], users[i]));
            }
            return list;
        }

        public static CreateRequestAchievementModel GetRequestAchievementModel(Guid achievementId = new Guid())
        {
            return new CreateRequestAchievementModel
            {
                AchievementId = achievementId,
                Message = RandomHelper.GetRandomString(100)
            };
        }

        public static ReadRequestAchievementModel GetRandomReadRequestAchievementModel()
        {
            return new ReadRequestAchievementModel
            {
                Id = Guid.NewGuid(),
                Message = RandomHelper.GetRandomString(100),
                AchievementName = RandomHelper.GetRandomString(100),
                UserName = RandomHelper.GetRandomString(100),
                AchievementId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
        }

        public static List<ReadRequestAchievementModel> GetReadRequestAchievementModels(
            List<RequestAchievement> models,
            List<Achievement> achievements,
            List<User> users)
        {
            var resultList = new List<ReadRequestAchievementModel>();
            for(int i = 0; i < models.Count; i++)
            {
                resultList.Add(GetReadRequestAchievementModel(models[i], achievements[i], users[i]));
            }
            return resultList;
        }

        public static ReadRequestAchievementModel GetReadRequestAchievementModel(RequestAchievement model, Achievement achievement, User user)
        {
            return new ReadRequestAchievementModel
            {
                Id = model.Id,
                Message = model.Message,
                AchievementName = achievement.Name,
                UserName = user.UserName,
                AchievementId = achievement.Id,
                UserId = user.Id
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
