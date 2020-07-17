using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class ChapterDumbData
    {
        #region Entity
        public static Chapter GetRandomEntity()
        {
            return new Chapter
            {
                OrderId = RandomHelper.GetRandomNumber(),
                Title = RandomHelper.GetRandomString(10),
                Articles = ArticleDumbData.GetRandomEntities(5)
            };
        }

        public static List<Chapter> GetRandomEntities(int number)
        {
            var list = new List<Chapter>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        #endregion
    }
}
