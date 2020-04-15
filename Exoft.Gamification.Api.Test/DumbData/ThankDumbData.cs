using Exoft.Gamification.Api.Common.Models.Thank;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class ThankDumbData
    {
        #region Entity
        public static Thank GetRandomEntity()
        {
            return new Thank
            {
                FromUser = UserDumbData.GetRandomEntity(),
                Text = RandomHelper.GetRandomString(50),
                ToUserId = Guid.NewGuid()
            };
        }

        public static List<Thank> GetRandomEntities(int number)
        {
            var list = new List<Thank>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        #endregion

        public static CreateThankModel GetCreateThankModel()
        {
            return new CreateThankModel
            {
                Text = RandomHelper.GetRandomString(50),
                ToUserId = Guid.NewGuid()
            };
        }
    }
}
