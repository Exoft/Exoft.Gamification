using Exoft.Gamification.Api.Common.Models.Achievement;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class AchievementDumbData
    {
        public static Achievement GetRandomEntity()
        {
            return new Achievement
            {
                Description = RandomHelper.GetRandomString(200),
                Name = RandomHelper.GetRandomString(20),
                XP = RandomHelper.GetRandomNumber()
            };
        }

        public static List<Achievement> GetRandomEntities(int number)
        {
            var list = new List<Achievement>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static Achievement GetEntity(UpdateAchievementModel updateModel)
        {
            return new Achievement
            {
                Name = updateModel.Name,
                Description = updateModel.Description,
                XP = updateModel.XP,
                IconId = Guid.NewGuid()
            };
        }

        public static ReadAchievementModel GetReadAchievementModel(CreateAchievementModel model)
        {
            return new ReadAchievementModel
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                XP = model.XP,
                IconId = Guid.NewGuid()
            };
        }

        public static CreateAchievementModel GetRandomCreateAchievementModel()
        {
            return new CreateAchievementModel
            {
                Name = RandomHelper.GetRandomString(20),
                Description = RandomHelper.GetRandomString(200),
                XP = RandomHelper.GetRandomNumber()
            };
        }

        public static UpdateAchievementModel GetRandomUpdateAchievementModel()
        {
            return new UpdateAchievementModel
            {
                Name = RandomHelper.GetRandomString(20),
                Description = RandomHelper.GetRandomString(200),
                XP = RandomHelper.GetRandomNumber()
            };
        }

    }
}
