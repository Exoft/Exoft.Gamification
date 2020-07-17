using Exoft.Gamification.Api.Common.Models.ReferenceBook;
using Exoft.Gamification.Api.Data.Core.Entities;
using Exoft.Gamification.Api.Test.TestData;
using System;
using System.Collections.Generic;

namespace Exoft.Gamification.Api.Test.DumbData
{
    public static class ArticleDumbData
    {
        #region Entity
        public static Article GetRandomEntity()
        {
            return new Article
            {
                Text = RandomHelper.GetRandomString(200),
                Title = RandomHelper.GetRandomString(10),
                UnitNumber = RandomHelper.GetRandomNumber()
            };
        }

        public static List<Article> GetRandomEntities(int number)
        {
            var list = new List<Article>();
            for (int i = 0; i < number; i++)
            {
                list.Add(GetRandomEntity());
            }
            return list;
        }

        public static Article GetEntity(UpdateArticleModel updateModel)
        {
            return new Article
            {
                Text = updateModel.Text,
                Title = updateModel.Title,
                UnitNumber = updateModel.UnitNumber
            };
        }

        #endregion

        public static UpdateArticleModel GetRandomUpdateModel()
        {
            return new UpdateArticleModel
            {
                Text = RandomHelper.GetRandomString(200),
                Title = RandomHelper.GetRandomString(10),
                UnitNumber = RandomHelper.GetRandomNumber()
            };
        }

        public static CreateChapterModel GetRandomCreateChapterModel()
        {
            return new CreateChapterModel
            {
                Title = RandomHelper.GetRandomString(10),
                Articles = new List<CreateArticleModel>
                {
                    GetRandomCreateArticleModel(),
                    GetRandomCreateArticleModel(),
                    GetRandomCreateArticleModel()
                }
            };
        }

        public static CreateArticleModel GetRandomCreateArticleModel()
        {
            return new CreateArticleModel
            {
                ChapterId = Guid.NewGuid(),
                Text = RandomHelper.GetRandomString(200),
                Title = RandomHelper.GetRandomString(10)
            };
        }
    }
}
